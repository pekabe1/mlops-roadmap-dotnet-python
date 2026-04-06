import type { CourseModule, CourseModuleDetail, ProgressRecord } from "@/types";

const BASE_URL = process.env.NEXT_PUBLIC_API_URL ?? "http://localhost:5000";

async function apiFetch<T>(path: string, init?: RequestInit): Promise<T> {
  const res = await fetch(`${BASE_URL}${path}`, {
    ...init,
    headers: { "Content-Type": "application/json", ...init?.headers },
  });
  if (!res.ok) throw new Error(`API error ${res.status}: ${path}`);
  return res.json() as Promise<T>;
}

export async function fetchModules(): Promise<CourseModule[]> {
  return apiFetch<CourseModule[]>("/api/course");
}

export async function fetchModule(id: string): Promise<CourseModuleDetail> {
  return apiFetch<CourseModuleDetail>(`/api/course/${id}`);
}

export async function fetchProgress(): Promise<ProgressRecord[]> {
  return apiFetch<ProgressRecord[]>("/api/progress");
}

export async function updateProgress(
  moduleId: string,
  completed: boolean
): Promise<void> {
  await apiFetch(`/api/progress/${moduleId}`, {
    method: "PUT",
    body: JSON.stringify({ completed }),
  });
}
