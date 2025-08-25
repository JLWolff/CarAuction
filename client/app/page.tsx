import { PublicAuctionInterface } from "@/components/public-auction-interface"

export default async function HomePage() {
  const user = {
    id: "1",
    name: "Joao Wolff",
    email: "joao.wolff@admin.com",
    role: "admin"
  }

  return <PublicAuctionInterface user={user} />
}
