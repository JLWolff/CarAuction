import { type NextRequest, NextResponse } from "next/server"
import { httpClient } from "@/lib/http-client"

export async function POST(request: NextRequest, { params }: { params: { id: string } }) {
  try {
    const response = await httpClient.post(`/api/auctions/${params.id}/close`)

    return NextResponse.json(response.data || { success: true })
  } catch (error: any) {
    console.error("Error closing auction:", error)
    const status = error.response?.status || 500
    const message = error.response?.data || { error: "Failed to close auction" }
    return NextResponse.json(message, { status })
  }
}
