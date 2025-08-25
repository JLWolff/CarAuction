"use client"

import { useState } from "react"
import { Button } from "@/components/ui/button"
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"
import { Badge } from "@/components/ui/badge"
import { useToast } from "@/hooks/use-toast"
import { Search, Filter, Car } from "lucide-react"

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

export function SearchFilters() {
  const [searchResults, setSearchResults] = useState<Vehicle[]>([])
  const [isLoading, setIsLoading] = useState(false)
  const [hasSearched, setHasSearched] = useState(false)
  const { toast } = useToast()
  const [filters, setFilters] = useState({
    type: "All Types",
    manufacturer: "",
    model: "",
    year: "",
  })

  const handleSearch = async () => {
    setIsLoading(true)
    setHasSearched(true)
    try {
      const params = new URLSearchParams()
      if (filters.type !== "All Types") params.append("type", filters.type)
      if (filters.manufacturer) params.append("manufacturer", filters.manufacturer)
      if (filters.model) params.append("model", filters.model)
      if (filters.year) params.append("year", filters.year)

      const response = await fetch(`/api/vehicles/search?${params.toString()}`)

      if (!response.ok) {
        const errorData = await response.json()
        throw new Error(errorData.message || "Search failed")
      }

      const results = await response.json()
      setSearchResults(results)

      toast({
        title: "Search Complete",
        description: `Found ${results.length} vehicle(s) matching your criteria.`,
      })
    } catch (error) {
      console.error("Search error:", error)
      toast({
        title: "Search Error",
        description: error instanceof Error ? error.message : "Failed to search vehicles. Please try again.",
        variant: "destructive",
      })
      setSearchResults([])
    } finally {
      setIsLoading(false)
    }
  }

  const clearFilters = () => {
    setFilters({
      type: "All Types",
      manufacturer: "",
      model: "",
      year: "",
    })
    setSearchResults([])
    setHasSearched(false)
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
          <CardTitle className="flex items-center gap-2">
            <Search className="h-5 w-5" />
            Vehicle Search & Filters
          </CardTitle>
        </CardHeader>
        <CardContent>
          <div className="space-y-6">
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
              <div className="space-y-2">
                <Label htmlFor="type">Vehicle Type</Label>
                <Select
                  value={filters.type}
                  onValueChange={(value) => setFilters((prev) => ({ ...prev, type: value }))}
                >
                  <SelectTrigger>
                    <SelectValue placeholder="Select type" />
                  </SelectTrigger>
                  <SelectContent>
                    <SelectItem value="All Types">All Types</SelectItem>
                    <SelectItem value="Sedan">Sedan</SelectItem>
                    <SelectItem value="SUV">SUV</SelectItem>
                    <SelectItem value="Hatchback">Hatchback</SelectItem>
                    <SelectItem value="Truck">Truck</SelectItem>
                  </SelectContent>
                </Select>
              </div>

              <div className="space-y-2">
                <Label htmlFor="manufacturer">Manufacturer</Label>
                <Input
                  id="manufacturer"
                  value={filters.manufacturer}
                  onChange={(e) => setFilters((prev) => ({ ...prev, manufacturer: e.target.value }))}
                  placeholder="e.g., Toyota, BMW"
                />
              </div>

              <div className="space-y-2">
                <Label htmlFor="model">Model</Label>
                <Input
                  id="model"
                  value={filters.model}
                  onChange={(e) => setFilters((prev) => ({ ...prev, model: e.target.value }))}
                  placeholder="e.g., Yaris, X5"
                />
              </div>

              <div className="space-y-2">
                <Label htmlFor="year">Year</Label>
                <Input
                  id="year"
                  type="number"
                  value={filters.year}
                  onChange={(e) => setFilters((prev) => ({ ...prev, year: e.target.value }))}
                  placeholder="e.g., 2022"
                  min="1900"
                  max={new Date().getFullYear() + 1}
                />
              </div>
            </div>

            <div className="flex gap-2">
              <Button onClick={handleSearch} disabled={isLoading}>
                <Search className="h-4 w-4 mr-2" />
                {isLoading ? "Searching..." : "Search Vehicles"}
              </Button>
              <Button variant="outline" onClick={clearFilters}>
                <Filter className="h-4 w-4 mr-2" />
                Clear Filters
              </Button>
            </div>
          </div>
        </CardContent>
      </Card>

      {searchResults.length > 0 && (
        <Card>
          <CardHeader>
            <CardTitle>Search Results ({searchResults.length} vehicles found)</CardTitle>
          </CardHeader>
          <CardContent>
            <div className="grid gap-4">
              {searchResults.map((vehicle) => (
                <Card key={vehicle.id} className="border-l-4 border-l-accent">
                  <CardContent className="pt-4">
                    <div className="flex items-center justify-between">
                      <div className="space-y-2">
                        <div className="flex items-center gap-2">
                          <h3 className="font-semibold">
                            {vehicle.manufacturer} {vehicle.model} ({vehicle.year})
                          </h3>
                          <Badge className={getTypeColor(vehicle.type)}>{vehicle.type}</Badge>
                        </div>
                        <div className="text-sm text-muted-foreground">
                          <p>Vehicle ID: {vehicle.id}</p>
                          <p>Starting Bid: ${vehicle.startingBid.toLocaleString()}</p>
                          {vehicle.numberOfDoors && <p>Doors: {vehicle.numberOfDoors}</p>}
                          {vehicle.numberOfSeats && <p>Seats: {vehicle.numberOfSeats}</p>}
                          {vehicle.loadCapacity && <p>Load Capacity: {vehicle.loadCapacity} tons</p>}
                        </div>
                      </div>
                      <Car className="h-8 w-8 text-muted-foreground" />
                    </div>
                  </CardContent>
                </Card>
              ))}
            </div>
          </CardContent>
        </Card>
      )}

      {searchResults.length === 0 && hasSearched && !isLoading && (
        <Card>
          <CardContent className="pt-6">
            <div className="text-center py-8 text-muted-foreground">
              <Search className="h-12 w-12 mx-auto mb-4 opacity-50" />
              <p>No vehicles found matching your search criteria.</p>
              <p className="text-sm">Try adjusting your filters and search again.</p>
            </div>
          </CardContent>
        </Card>
      )}
    </div>
  )
}
