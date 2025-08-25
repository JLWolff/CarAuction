import { type NextRequest, NextResponse } from "next/server"
import { httpClient } from "@/lib/http-client"

export async function POST(request: NextRequest) {
  try {
    const body = await request.json()

    const response = await httpClient.post("/api/auctions", body)

    return NextResponse.json(response.data)
  } catch (error: any) {
    console.error("Error starting auction:", error)
    const status = error.response?.status || 500
    const message = error.response?.data || { error: "Failed to start auction" }
    return NextResponse.json(message, { status })
  }
}

export async function GET(request: NextRequest) {
  try {
    const response = await httpClient.get("/api/auctions")

    return NextResponse.json(response.data)
  } catch (error: any) {
    console.error("Error fetching auctions:", error)
    const status = error.response?.status || 500
    const message = error.response?.data || { error: "Failed to fetch auctions" }
    return NextResponse.json(message, { status })
  }
}
