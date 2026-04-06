import type { Metadata } from "next";
import "./globals.css";

export const metadata: Metadata = {
  title: "MLOps Roadmap: .NET & Python",
  description:
    "An interactive educational platform for .NET developers learning MLOps with Python",
};

export default function RootLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <html lang="en">
      <body className="min-h-screen bg-gray-950 text-gray-100 antialiased">
        {children}
      </body>
    </html>
  );
}
