"use client"

import { useState } from "react"
import { VehicleInventory } from "@/components/vehicle-inventory"
import { AuctionManagement } from "@/components/auction-management"
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs"
import { Button } from "@/components/ui/button"
import { Car, Gavel, Shield } from "lucide-react"

interface User {
  id: string
  name: string
  email: string
  role: string
}

interface AdminDashboardProps {
  user: User
}

export function AdminDashboard({ user }: AdminDashboardProps) {
  const [activeTab, setActiveTab] = useState("inventory")

  return (
    <div className="min-h-screen bg-background">
      {/* Header */}
      <header className="border-b bg-primary text-primary-foreground">
        <div className="container mx-auto px-4 py-4">
          <div className="flex items-center justify-between">
            <div className="flex items-center gap-3">
              <Shield className="h-8 w-8" />
              <h1 className="text-2xl font-bold">AutoAuction Pro - Admin</h1>
            </div>
            <div className="flex items-center gap-4">
              <span className="text-sm opacity-90">Welcome, {user.name}</span>
            </div>
          </div>
        </div>
      </header>

      {/* Main Content */}
      <main className="container mx-auto px-4 py-6">
        <Tabs value={activeTab} onValueChange={setActiveTab} className="space-y-6">
          <TabsList className="grid w-full grid-cols-2">
            <TabsTrigger value="inventory" className="flex items-center gap-2">
              <Car className="h-4 w-4" />
              Vehicle Inventory
            </TabsTrigger>
            <TabsTrigger value="auctions" className="flex items-center gap-2">
              <Gavel className="h-4 w-4" />
              Auction Management
            </TabsTrigger>
          </TabsList>

          <TabsContent value="inventory" className="space-y-6">
            <VehicleInventory />
          </TabsContent>

          <TabsContent value="auctions" className="space-y-6">
            <AuctionManagement />
          </TabsContent>
        </Tabs>
      </main>
    </div>
  )
}
