import { type NextRequest, NextResponse } from "next/server"
import { httpClient } from "@/lib/http-client"

export async function GET(request: NextRequest) {
  try {
    const { searchParams } = new URL(request.url)
    const queryString = searchParams.toString()

    const response = await httpClient.get(`/api/vehiclesInventory/search?${queryString}`);

    return NextResponse.json(response.data)
  } catch (error: any) {
    console.error("Error searching vehicles:", error)
    const status = error.response?.status || 500
    const message = error.response?.data || { error: "Failed to search vehicles" }
    return NextResponse.json(message, { status })
  }
}
