import { AdminDashboard } from "@/components/admin-dashboard"

export default async function AdminPage() {
  const user = {
    id: "1",
    name: "Joao Wolff",
    email: "joao.wolff@admin.com",
    role: "admin"
  }
  return <AdminDashboard user={user} />
}
