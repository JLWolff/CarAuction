"use client"

import { useState, useEffect } from "react"
import { Button } from "@/components/ui/button"
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Badge } from "@/components/ui/badge"
import { useToast } from "@/hooks/use-toast"
import { TrendingUp, DollarSign, User, RefreshCw } from "lucide-react"

interface Bid {
  id: string
  auctionId: string
  bidderId: string
  amount: number
  timestamp: string
}

interface ActiveAuction {
  id: string
  vehicleId: string
  vehicle: {
    manufacturer: string
    model: string
    year: number
    type: string
  }
  status: string
  currentHighestBid: number
  highestBidderId?: string
  startTime: string
  bids?: Bid[]
}

interface BiddingInterfaceProps {
  userId: string
}

export function BiddingInterface({ userId }: BiddingInterfaceProps) {
  const [activeAuctions, setActiveAuctions] = useState<ActiveAuction[]>([])
  const [loading, setLoading] = useState(false)
  const [bidAmounts, setBidAmounts] = useState<Record<string, number>>({})
  const { toast } = useToast()

  useEffect(() => {
    loadActiveAuctions()
  }, [])

  const loadActiveAuctions = async () => {
    try {
      setLoading(true)
      const response = await fetch("/api/auctions")
      if (response.ok) {
        const auctions = await response.json()
        const activeOnly = auctions.filter((auction: ActiveAuction) => auction.status === "Active")
        setActiveAuctions(activeOnly)
      }
    } catch (error) {
      console.error("Failed to load auctions:", error)
      toast({
        title: "Error",
        description: "Failed to load active auctions.",
        variant: "destructive",
      })
    } finally {
      setLoading(false)
    }
  }

  const loadAuctionBids = async (auctionId: string) => {
    try {
      const response = await fetch(`/api/auctions/${auctionId}/bids`)
      if (response.ok) {
        const bids = await response.json()
        setActiveAuctions((prev) => prev.map((auction) => (auction.id === auctionId ? { ...auction, bids } : auction)))
      }
    } catch (error) {
      console.error("Failed to load bids:", error)
    }
  }

  const handlePlaceBid = async (auctionId: string) => {
    try {
      const bidAmount = bidAmounts[auctionId]
      if (!bidAmount || bidAmount <= 0) {
        toast({
          title: "Invalid Bid",
          description: "Please enter a valid bid amount.",
          variant: "destructive",
        })
        return
      }

      const auction = activeAuctions.find((a) => a.id === auctionId)
      if (!auction) {
        toast({
          title: "Auction Not Found",
          description: "The specified auction could not be found.",
          variant: "destructive",
        })
        return
      }

      if (bidAmount <= auction.currentHighestBid) {
        toast({
          title: "Bid Too Low",
          description: `Bid must be higher than current highest bid of $${auction.currentHighestBid.toLocaleString()}.`,
          variant: "destructive",
        })
        return
      }

      const response = await fetch(`/api/auctions/${auctionId}/bids`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          bidderId: userId,
          amount: bidAmount,
        }),
      })

      if (!response.ok) {
        const errorData = await response.json()
        throw new Error(errorData.message || "Failed to place bid")
      }

      const updatedAuction = await response.json()

      setActiveAuctions((prev) => prev.map((auction) => (auction.id === auctionId ? updatedAuction : auction)))

      setBidAmounts((prev) => ({ ...prev, [auctionId]: 0 }))

      toast({
        title: "Bid Placed Successfully",
        description: `Your bid of $${bidAmount.toLocaleString()} has been placed.`,
      })

      // Load updated bids
      await loadAuctionBids(auctionId)
    } catch (error) {
      toast({
        title: "Error",
        description: error instanceof Error ? error.message : "Failed to place bid. Please try again.",
        variant: "destructive",
      })
    }
  }

  const setBidAmount = (auctionId: string, amount: number) => {
    setBidAmounts((prev) => ({ ...prev, [auctionId]: amount }))
  }

  return (
    <div className="space-y-6">
      <Card>
        <CardHeader>
          <div className="flex items-center justify-between">
            <CardTitle className="flex items-center gap-2">
              <TrendingUp className="h-5 w-5" />
              Live Auctions
            </CardTitle>
            <Button variant="outline" onClick={loadActiveAuctions} disabled={loading}>
              <RefreshCw className={`h-4 w-4 mr-2 ${loading ? "animate-spin" : ""}`} />
              Refresh
            </Button>
          </div>
        </CardHeader>
        <CardContent>
          {loading ? (
            <div className="text-center py-8 text-muted-foreground">
              <p>Loading active auctions...</p>
            </div>
          ) : activeAuctions.length === 0 ? (
            <div className="text-center py-8 text-muted-foreground">
              <TrendingUp className="h-12 w-12 mx-auto mb-4 opacity-50" />
              <p>No active auctions available for bidding.</p>
            </div>
          ) : (
            <div className="space-y-6">
              {activeAuctions.map((auction) => (
                <Card key={auction.id} className="border-l-4 border-l-green-500">
                  <CardContent className="pt-6">
                    <div className="space-y-4">
                      <div className="flex items-center justify-between">
                        <div>
                          <h3 className="text-lg font-semibold">
                            {auction.vehicle.manufacturer} {auction.vehicle.model} ({auction.vehicle.year})
                          </h3>
                          <p className="text-sm text-muted-foreground">
                            Vehicle ID: {auction.vehicleId} • Type: {auction.vehicle.type}
                          </p>
                        </div>
                        <Badge className="bg-green-100 text-green-800">Live Auction</Badge>
                      </div>

                      <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                        <div className="space-y-4">
                          <div className="flex items-center gap-2 text-2xl font-bold text-green-600">
                            <DollarSign className="h-6 w-6" />
                            {auction.currentHighestBid.toLocaleString()}
                          </div>
                          {auction.highestBidderId && (
                            <div className="flex items-center gap-2 text-sm text-muted-foreground">
                              <User className="h-4 w-4" />
                              Current leader: {auction.highestBidderId}
                            </div>
                          )}

                          <div className="space-y-2">
                            <div className="flex items-center justify-between">
                              <h4 className="font-medium">Recent Bids</h4>
                              <Button variant="ghost" size="sm" onClick={() => loadAuctionBids(auction.id)}>
                                <RefreshCw className="h-3 w-3" />
                              </Button>
                            </div>
                            <div className="space-y-1 max-h-32 overflow-y-auto">
                              {auction.bids && auction.bids.length > 0 ? (
                                auction.bids
                                  .slice(-5)
                                  .reverse()
                                  .map((bid) => (
                                    <div key={bid.id} className="flex justify-between text-sm">
                                      <span className={bid.bidderId === userId ? "font-semibold text-primary" : ""}>
                                        {bid.bidderId === userId ? "You" : bid.bidderId}
                                      </span>
                                      <span className="font-medium">${bid.amount.toLocaleString()}</span>
                                    </div>
                                  ))
                              ) : (
                                <p className="text-sm text-muted-foreground">No bids yet</p>
                              )}
                            </div>
                          </div>
                        </div>

                        <div className="space-y-4">
                          <h4 className="font-medium">Place Your Bid</h4>
                          <div className="space-y-3">
                            <div className="space-y-2">
                              <Label htmlFor={`amount-${auction.id}`}>Bid Amount ($)</Label>
                              <Input
                                id={`amount-${auction.id}`}
                                type="number"
                                value={bidAmounts[auction.id] || ""}
                                onChange={(e) => setBidAmount(auction.id, Number.parseFloat(e.target.value) || 0)}
                                placeholder={`Minimum: $${(auction.currentHighestBid + 1).toLocaleString()}`}
                                min={auction.currentHighestBid + 1}
                              />
                            </div>
                            <Button
                              onClick={() => handlePlaceBid(auction.id)}
                              className="w-full"
                              size="lg"
                              disabled={!bidAmounts[auction.id] || bidAmounts[auction.id] <= auction.currentHighestBid}
                            >
                              <TrendingUp className="h-4 w-4 mr-2" />
                              Place Bid
                            </Button>
                          </div>
                        </div>
                      </div>
                    </div>
                  </CardContent>
                </Card>
              ))}
            </div>
          )}
        </CardContent>
      </Card>
    </div>
  )
}
