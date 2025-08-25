"use client"

import { useState, useEffect } from "react"
import { Button } from "@/components/ui/button"
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"
import { Badge } from "@/components/ui/badge"
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogTrigger } from "@/components/ui/dialog"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"
import { useToast } from "@/hooks/use-toast"
import { Gavel, Play, Square, Clock } from "lucide-react"

interface Auction {
  id: string
  vehicleId: string
  vehicle: {
    id: string
    manufacturer: string
    model: string
    year: number
    type: string
  }
  status: "Active" | "Draft" | "Closed"
  currentHighestBid: number
  highestBidderId?: string
  startTime: string
  endTime?: string
}

export function AuctionManagement() {
  const [auctions, setAuctions] = useState<Auction[]>([])
  const [vehicles, setVehicles] = useState<any[]>([])
  const [loadingAuctions, setLoadingAuctions] = useState(false)
  const [selectedVehicleId, setSelectedVehicleId] = useState<string>("")
  const [isDialogOpen, setIsDialogOpen] = useState(false)
  const { toast } = useToast()

  useEffect(() => {
    loadAuctions()
    loadVehicles()
  }, [])

  const loadAuctions = async () => {
    try {
      setLoadingAuctions(true)
      const response = await fetch("/api/auctions")
      if (response.ok) {
        const data = await response.json()
        setAuctions(data)
      }
    } catch (error) {
      console.error("Failed to load auctions:", error)
    } finally {
      setLoadingAuctions(false)
    }
  }

  const loadVehicles = async () => {
    try {
      const response = await fetch("/api/vehicles/search")
      if (response.ok) {
        const data = await response.json()
        setVehicles(data)
      }
    } catch (error) {
      console.error("Failed to load vehicles:", error)
    }
  }

  const getStatusColor = (status: Auction["status"]) => {
    const colors = {
      Active: "bg-green-100 text-green-800",
      Draft: "bg-yellow-100 text-yellow-800",
      Closed: "bg-gray-100 text-gray-800",
    }
    return colors[status]
  }

  const handleStartAuction = async (vehicleId: string) => {
    try {
      const existingAuction = auctions.find((a) => a.vehicleId === vehicleId && a.status === "Active")
      if (existingAuction) {
        toast({
          title: "Error",
          description: "This vehicle already has an active auction.",
          variant: "destructive",
        })
        return
      }

      const response = await fetch("/api/auctions", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ vehicleId }),
      })

      if (!response.ok) {
        const errorData = await response.json()
        throw new Error(errorData.message || "Failed to start auction")
      }

      const newAuction = await response.json()
      setAuctions((prev) => [...prev, newAuction])
      setIsDialogOpen(false)
      setSelectedVehicleId("")

      toast({
        title: "Auction Started",
        description: `Auction for vehicle ${vehicleId} has been started successfully.`,
      })
    } catch (error) {
      toast({
        title: "Error",
        description:
          error instanceof Error
            ? error.message
            : "Failed to start auction. Please ensure the vehicle exists and is not already in an auction.",
        variant: "destructive",
      })
    }
  }

  const handleCloseAuction = async (auctionId: string) => {
    try {
      const response = await fetch(`/api/auctions/${auctionId}/close`, {
        method: "POST",
      })

      if (!response.ok) {
        const errorData = await response.json()
        throw new Error(errorData.message || "Failed to close auction")
      }

      await loadAuctions()

      toast({
        title: "Auction Closed",
        description: "Auction has been closed successfully.",
      })
    } catch (error) {
      toast({
        title: "Error",
        description: error instanceof Error ? error.message : "Failed to close auction. Please try again.",
        variant: "destructive",
      })
    }
  }

  return (
    <div className="space-y-6">
      <Card>
        <CardHeader>
          <CardTitle className="flex items-center gap-2">
            <Gavel className="h-5 w-5" />
            Auction Management
          </CardTitle>
        </CardHeader>
        <CardContent>
          <div className="space-y-4">
            <div className="flex gap-4">
              <Dialog open={isDialogOpen} onOpenChange={setIsDialogOpen}>
                <DialogTrigger asChild>
                  <Button>
                    <Play className="h-4 w-4 mr-2" />
                    Start New Auction
                  </Button>
                </DialogTrigger>
                <DialogContent>
                  <DialogHeader>
                    <DialogTitle>Start New Auction</DialogTitle>
                  </DialogHeader>
                  <div className="space-y-4">
                    <div className="space-y-2">
                      <label className="text-sm font-medium">Select Vehicle</label>
                      <Select value={selectedVehicleId} onValueChange={setSelectedVehicleId}>
                        <SelectTrigger>
                          <SelectValue placeholder="Choose a vehicle to auction" />
                        </SelectTrigger>
                        <SelectContent>
                          {vehicles
                            .filter(
                              (vehicle) =>
                                !auctions.some(
                                  (auction) => auction.vehicleId === vehicle.id && auction.status === "Active",
                                ),
                            )
                            .map((vehicle) => (
                              <SelectItem key={vehicle.id} value={vehicle.id}>
                                {vehicle.manufacturer} {vehicle.model} ({vehicle.year}) - ID: {vehicle.id}
                              </SelectItem>
                            ))}
                        </SelectContent>
                      </Select>
                    </div>
                    <div className="flex gap-2 justify-end">
                      <Button variant="outline" onClick={() => setIsDialogOpen(false)}>
                        Cancel
                      </Button>
                      <Button
                        onClick={() => selectedVehicleId && handleStartAuction(selectedVehicleId)}
                        disabled={!selectedVehicleId}
                      >
                        Start Auction
                      </Button>
                    </div>
                  </div>
                </DialogContent>
              </Dialog>
              <Button variant="outline" onClick={loadAuctions} disabled={loadingAuctions}>
                {loadingAuctions ? "Loading..." : "Refresh Auctions"}
              </Button>
            </div>

            {loadingAuctions ? (
              <div className="text-center py-8 text-muted-foreground">
                <p>Loading auctions...</p>
              </div>
            ) : auctions.length === 0 ? (
              <div className="text-center py-8 text-muted-foreground">
                <Gavel className="h-12 w-12 mx-auto mb-4 opacity-50" />
                <p>No auctions created yet. Start your first auction to begin.</p>
              </div>
            ) : (
              <div className="grid gap-4">
                {auctions.map((auction) => (
                  <Card key={auction.id} className="border-l-4 border-l-accent">
                    <CardContent className="pt-6">
                      <div className="flex items-center justify-between">
                        <div className="space-y-2">
                          <div className="flex items-center gap-2">
                            <h3 className="font-semibold">
                              {auction.vehicle?.manufacturer} {auction.vehicle?.model} ({auction.vehicle?.year})
                            </h3>
                            <Badge className={getStatusColor(auction.status)}>{auction.status}</Badge>
                          </div>
                          <div className="text-sm text-muted-foreground">
                            <p>Vehicle ID: {auction.vehicleId}</p>
                            <p>Auction ID: {auction.id}</p>
                            <p>Current Highest Bid: ${auction.currentHighestBid.toLocaleString()}</p>
                            {auction.highestBidderId && <p>Highest Bidder: {auction.highestBidderId}</p>}
                          </div>
                          <div className="flex items-center gap-4 text-sm text-muted-foreground">
                            <div className="flex items-center gap-1">
                              <Clock className="h-4 w-4" />
                              Started: {new Date(auction.startTime).toLocaleString()}
                            </div>
                            {auction.endTime && (
                              <div className="flex items-center gap-1">
                                <Clock className="h-4 w-4" />
                                Ended: {new Date(auction.endTime).toLocaleString()}
                              </div>
                            )}
                          </div>
                        </div>
                        <div className="flex gap-2">
                          {auction.status === "Active" && (
                            <Button variant="outline" onClick={() => handleCloseAuction(auction.id)}>
                              <Square className="h-4 w-4 mr-2" />
                              Close Auction
                            </Button>
                          )}
                        </div>
                      </div>
                    </CardContent>
                  </Card>
                ))}
              </div>
            )}
          </div>
        </CardContent>
      </Card>
    </div>
  )
}
