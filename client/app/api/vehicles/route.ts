import { type NextRequest, NextResponse } from "next/server"
import { httpClient } from "@/lib/http-client"

export async function POST(request: NextRequest) {
  try {
    const body = await request.json()

    const response = await httpClient.post("/api/vehiclesInventory", body)

    return NextResponse.json(response.data)
  } catch (error: any) {
    console.error("Error adding vehicle:", error)
    const status = error.response?.status || 500
    const message = error.response?.data || { error: "Failed to add vehicle" }
    return NextResponse.json(message, { status })
  }
}
