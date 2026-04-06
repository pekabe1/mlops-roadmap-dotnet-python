import type { ProgressMap } from "@/types";

const STORAGE_KEY = "mlops-roadmap-progress";

export function loadLocalProgress(): ProgressMap {
  if (typeof window === "undefined") return {};
  try {
    const raw = localStorage.getItem(STORAGE_KEY);
    return raw ? (JSON.parse(raw) as ProgressMap) : {};
  } catch {
    return {};
  }
}

export function saveLocalProgress(progress: ProgressMap): void {
  if (typeof window === "undefined") return;
  try {
    localStorage.setItem(STORAGE_KEY, JSON.stringify(progress));
  } catch {
    // ignore storage errors
  }
}

export function setLocalModuleProgress(
  moduleId: string,
  completed: boolean
): ProgressMap {
  const current = loadLocalProgress();
  const updated = { ...current, [moduleId]: completed };
  saveLocalProgress(updated);
  return updated;
}
