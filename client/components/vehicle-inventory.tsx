"use client"

import type React from "react"

import { useEffect, useState } from "react"
import { Button } from "@/components/ui/button"
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table"
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogTrigger } from "@/components/ui/dialog"
import { Badge } from "@/components/ui/badge"
import { useToast } from "@/hooks/use-toast"
import { Plus, Edit, Trash2, Car } from "lucide-react"

interface Vehicle {
  id: string
  manufacturer: string
  model: string
  year: number
  type: "Sedan" | "SUV" | "Hatchback" | "Truck"
  startingBid: number
  numberOfDoors?: number
  numberOfSeats?: number
  loadCapacity?: number
}

export function VehicleInventory() {
  const [vehicles, setVehicles] = useState<Vehicle[]>([])
  const [isAddDialogOpen, setIsAddDialogOpen] = useState(false)
  const { toast } = useToast()

  useEffect(() => {
      loadVehicles()
    }, [])
  
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

  const [formData, setFormData] = useState({
    id: "",
    manufacturer: "",
    model: "",
    year: new Date().getFullYear(),
    type: "Sedan" as Vehicle["type"],
    startingBid: 0,
    numberOfDoors: undefined as number | undefined,
    numberOfSeats: undefined as number | undefined,
    loadCapacity: undefined as number | undefined,
  })

  const resetForm = () => {
    setFormData({
      id: "",
      manufacturer: "",
      model: "",
      year: new Date().getFullYear(),
      type: "Sedan",
      startingBid: 0,
      numberOfDoors: undefined,
      numberOfSeats: undefined,
      loadCapacity: undefined,
    })
  }

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()

    try {
      if (vehicles.some((v) => v.id === formData.id)) {
        toast({
          title: "Duplicate Vehicle ID",
          description: "A vehicle with this ID already exists in the inventory.",
          variant: "destructive",
        })
        return
      }

      const vehicleData = {
        ...formData,
        numberOfDoors: formData.numberOfDoors || undefined,
        numberOfSeats: formData.numberOfSeats || undefined,
        loadCapacity: formData.loadCapacity || undefined,
      }

      const response = await fetch("/api/vehicles", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(vehicleData),
      })

      if (!response.ok) {
        const errorData = await response.json()
        throw new Error(errorData.message || "Failed to add vehicle")
      }

      const savedVehicle = await response.json()

    
      setVehicles((prev) => [...prev, savedVehicle])
      toast({
        title: "Vehicle Added",
        description: "Vehicle has been successfully added to inventory.",
      })

      setIsAddDialogOpen(false)
      resetForm()
    } catch (error) {
      toast({
        title: "Error",
        description: error instanceof Error ? error.message : "Failed to save vehicle. Please try again.",
        variant: "destructive",
      })
    }
  }

  const getTypeColor = (type: Vehicle["type"]) => {
    const colors = {
      Sedan: "bg-blue-100 text-blue-800",
      SUV: "bg-green-100 text-green-800",
      Hatchback: "bg-purple-100 text-purple-800",
      Truck: "bg-orange-100 text-orange-800",
    }
    return colors[type]
  }

  return (
    <div className="space-y-6">
      <Card>
        <CardHeader>
          <div className="flex items-center justify-between">
            <CardTitle className="flex items-center gap-2">
              <Car className="h-5 w-5" />
              Vehicle Inventory
            </CardTitle>
            <Dialog open={isAddDialogOpen} onOpenChange={setIsAddDialogOpen}>
              <DialogTrigger asChild>
                <Button onClick={resetForm}>
                  <Plus className="h-4 w-4 mr-2" />
                  Add Vehicle
                </Button>
              </DialogTrigger>
              <DialogContent className="max-w-2xl">
                <DialogHeader>
                  <DialogTitle>{"Add New Vehicle"}</DialogTitle>
                </DialogHeader>
                <form onSubmit={handleSubmit} className="space-y-4">
                  <div className="grid grid-cols-2 gap-4">
                    <div className="space-y-2">
                      <Label htmlFor="id">Vehicle ID *</Label>
                      <Input
                        id="id"
                        value={formData.id}
                        onChange={(e) => setFormData((prev) => ({ ...prev, id: e.target.value }))}
                        placeholder="Enter unique vehicle ID"
                        required
                      />
                    </div>
                    <div className="space-y-2">
                      <Label htmlFor="type">Type *</Label>
                      <Select
                        value={formData.type}
                        onValueChange={(value: Vehicle["type"]) => setFormData((prev) => ({ ...prev, type: value }))}
                      >
                        <SelectTrigger>
                          <SelectValue />
                        </SelectTrigger>
                        <SelectContent>
                          <SelectItem value="Sedan">Sedan</SelectItem>
                          <SelectItem value="SUV">SUV</SelectItem>
                          <SelectItem value="Hatchback">Hatchback</SelectItem>
                          <SelectItem value="Truck">Truck</SelectItem>
                        </SelectContent>
                      </Select>
                    </div>
                  </div>

                  <div className="grid grid-cols-2 gap-4">
                    <div className="space-y-2">
                      <Label htmlFor="manufacturer">Manufacturer *</Label>
                      <Input
                        id="manufacturer"
                        value={formData.manufacturer}
                        onChange={(e) => setFormData((prev) => ({ ...prev, manufacturer: e.target.value }))}
                        placeholder="e.g., Toyota, BMW"
                        required
                      />
                    </div>
                    <div className="space-y-2">
                      <Label htmlFor="model">Model *</Label>
                      <Input
                        id="model"
                        value={formData.model}
                        onChange={(e) => setFormData((prev) => ({ ...prev, model: e.target.value }))}
                        placeholder="e.g., Camry, X5"
                        required
                      />
                    </div>
                  </div>

                  <div className="grid grid-cols-2 gap-4">
                    <div className="space-y-2">
                      <Label htmlFor="year">Year</Label>
                      <Input
                        id="year"
                        type="number"
                        value={formData.year}
                        onChange={(e) => setFormData((prev) => ({ ...prev, year: Number.parseInt(e.target.value) }))}
                        min="1900"
                        max={new Date().getFullYear() + 1}
                      />
                    </div>
                    <div className="space-y-2">
                      <Label htmlFor="startingBid">Starting Bid ($)</Label>
                      <Input
                        id="startingBid"
                        type="number"
                        value={formData.startingBid}
                        onChange={(e) =>
                          setFormData((prev) => ({ ...prev, startingBid: Number.parseFloat(e.target.value) }))
                        }
                        min="0.1"
                        step="0.01"
                      />
                    </div>
                  </div>

                  {(formData.type === "Sedan" || formData.type === "SUV" || formData.type === "Hatchback") && (
                    <div className="grid grid-cols-2 gap-4">
                      <div className="space-y-2">
                        <Label htmlFor="doors">Number of Doors</Label>
                        <Input
                          id="doors"
                          type="number"
                          value={formData.numberOfDoors || ""}
                          onChange={(e) =>
                            setFormData((prev) => ({
                              ...prev,
                              numberOfDoors: e.target.value ? Number.parseInt(e.target.value) : undefined,
                            }))
                          }
                          min="2"
                          max="5"
                        />
                      </div>
                      <div className="space-y-2">
                        <Label htmlFor="seats">Number of Seats</Label>
                        <Input
                          id="seats"
                          type="number"
                          value={formData.numberOfSeats || ""}
                          onChange={(e) =>
                            setFormData((prev) => ({
                              ...prev,
                              numberOfSeats: e.target.value ? Number.parseInt(e.target.value) : undefined,
                            }))
                          }
                          min="2"
                          max="9"
                        />
                      </div>
                    </div>
                  )}

                  {formData.type === "Truck" && (
                    <div className="space-y-2">
                      <Label htmlFor="loadCapacity">Load Capacity (tons)</Label>
                      <Input
                        id="loadCapacity"
                        type="number"
                        value={formData.loadCapacity || ""}
                        onChange={(e) =>
                          setFormData((prev) => ({
                            ...prev,
                            loadCapacity: e.target.value ? Number.parseFloat(e.target.value) : undefined,
                          }))
                        }
                        min="0.1"
                        step="0.1"
                      />
                    </div>
                  )}

                  <div className="flex justify-end gap-2">
                    <Button
                      type="button"
                      variant="outline"
                      onClick={() => {
                        setIsAddDialogOpen(false)
                        resetForm()
                      }}
                    >
                      Cancel
                    </Button>
                    <Button type="submit">Add Vehicle</Button>
                  </div>
                </form>
              </DialogContent>
            </Dialog>
          </div>
        </CardHeader>
        <CardContent>
          {vehicles.length === 0 ? (
            <div className="text-center py-8 text-muted-foreground">
              <Car className="h-12 w-12 mx-auto mb-4 opacity-50" />
              <p>No vehicles in inventory. Add your first vehicle to get started.</p>
            </div>
          ) : (
            <Table>
              <TableHeader>
                <TableRow>
                  <TableHead>ID</TableHead>
                  <TableHead>Type</TableHead>
                  <TableHead>Manufacturer</TableHead>
                  <TableHead>Model</TableHead>
                  <TableHead>Year</TableHead>
                  <TableHead>Starting Bid</TableHead>
                </TableRow>
              </TableHeader>
              <TableBody>
                {vehicles.map((vehicle) => (
                  <TableRow key={vehicle.id}>
                    <TableCell className="font-medium">{vehicle.id}</TableCell>
                    <TableCell>
                      <Badge className={getTypeColor(vehicle.type)}>{vehicle.type}</Badge>
                    </TableCell>
                    <TableCell>{vehicle.manufacturer}</TableCell>
                    <TableCell>{vehicle.model}</TableCell>
                    <TableCell>{vehicle.year}</TableCell>
                    <TableCell>${vehicle.startingBid.toLocaleString()}</TableCell>
                   
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          )}
        </CardContent>
      </Card>
    </div>
  )
}
