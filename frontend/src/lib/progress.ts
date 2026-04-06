import type { CourseModule, ProgressMap } from "@/types";

const STORAGE_KEY = "mlops-roadmap-progress";
const MODULES_KEY = "mlops-roadmap-modules";

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

export function saveLocalModules(modules: CourseModule[]): void {
  if (typeof window === "undefined") return;
  try {
    localStorage.setItem(MODULES_KEY, JSON.stringify(modules));
  } catch {
    // ignore storage errors
  }
}

export function loadLocalModules(): CourseModule[] {
  if (typeof window === "undefined") return [];
  try {
    const raw = localStorage.getItem(MODULES_KEY);
    return raw ? (JSON.parse(raw) as CourseModule[]) : [];
  } catch {
    return [];
  }
}
