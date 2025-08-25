import { type NextRequest, NextResponse } from "next/server"
import { httpClient } from "@/lib/http-client"

export async function POST(request: NextRequest, { params }: { params: { id: string } }) {
  try {
    const body = await request.json()

    //simulating session for authentication
    const session = {
      user: {
        id: "1",
        name: "Joao Wolff",
        email: "joao.wolff@admin.com",
        role: "admin"
      }
    }

    const bidData = {
      ...body,
      bidderId: session.user.id,
    }

    const response = await httpClient.post(`/api/auctions/${params.id}/bids`, bidData)

    return NextResponse.json(response.data)
  } catch (error: any) {
    console.error("Error placing bid:", error)
    const status = error.response?.status || 500
    const message = error.response?.data || { error: "Failed to place bid" }
    return NextResponse.json(message, { status })
  }
}

export async function GET(request: NextRequest, { params }: { params: { id: string } }) {
  try {
    const response = await httpClient.get(`/api/auctions/${params.id}/bids`)

    return NextResponse.json(response.data)
  } catch (error: any) {
    console.error("Error fetching bids:", error)
    const status = error.response?.status || 500
    const message = error.response?.data || { error: "Failed to fetch bids" }
    return NextResponse.json(message, { status })
  }
}
