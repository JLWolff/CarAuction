"use client"

import { useState } from "react"
import { BiddingInterface } from "@/components/bidding-interface"
import { SearchFilters } from "@/components/search-filters"
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs"
import { Button } from "@/components/ui/button"
import { Car, Search, TrendingUp, LogOut } from "lucide-react"

interface User {
  id: string
  name: string
  email: string
  role: string
}

interface PublicAuctionInterfaceProps {
  user: User
}

export function PublicAuctionInterface({ user }: PublicAuctionInterfaceProps) {
  const [activeTab, setActiveTab] = useState("auctions")

  const handleLogout = () => {
    window.location.href = "/auth/signout"
  }

  return (
    <div className="min-h-screen bg-background">
      {/* Header */}
      <header className="border-b bg-primary text-primary-foreground">
        <div className="container mx-auto px-4 py-4">
          <div className="flex items-center justify-between">
            <div className="flex items-center gap-3">
              <Car className="h-8 w-8" />
              <h1 className="text-2xl font-bold">AutoAuction Pro</h1>
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
            <TabsTrigger value="auctions" className="flex items-center gap-2">
              <TrendingUp className="h-4 w-4" />
              Live Auctions
            </TabsTrigger>
            <TabsTrigger value="search" className="flex items-center gap-2">
              <Search className="h-4 w-4" />
              Search Vehicles
            </TabsTrigger>
          </TabsList>

          <TabsContent value="auctions" className="space-y-6">
            <BiddingInterface userId={user.id} />
          </TabsContent>

          <TabsContent value="search" className="space-y-6">
            <SearchFilters />
          </TabsContent>
        </Tabs>
      </main>
    </div>
  )
}
