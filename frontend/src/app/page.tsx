"use client";

import { useEffect, useState, useCallback } from "react";
import { BookOpen, Wifi, WifiOff } from "lucide-react";
import type { CourseModule, ProgressMap } from "@/types";
import { fetchModules, fetchProgress, updateProgress } from "@/lib/api";
import {
  loadLocalProgress,
  setLocalModuleProgress,
  saveLocalModules,
  loadLocalModules,
} from "@/lib/progress";
import RoadmapView from "@/components/RoadmapView";
import LessonView from "@/components/LessonView";

type View = { type: "roadmap" } | { type: "lesson"; moduleId: string };

export default function HomePage() {
  const [modules, setModules] = useState<CourseModule[]>([]);
  const [progress, setProgress] = useState<ProgressMap>({});
  const [view, setView] = useState<View>({ type: "roadmap" });
  const [loading, setLoading] = useState(true);
  const [backendOnline, setBackendOnline] = useState(false);

  const loadData = useCallback(async () => {
    setLoading(true);
    try {
      const [mods, prog] = await Promise.all([fetchModules(), fetchProgress()]);
      setModules(mods);
      saveLocalModules(mods);
      const map: ProgressMap = {};
      prog.forEach((r) => {
        map[r.moduleId] = r.completed;
      });
      setProgress(map);
      setBackendOnline(true);
    } catch {
      // Backend unavailable — fall back to localStorage
      setBackendOnline(false);
      setModules(loadLocalModules());
      setProgress(loadLocalProgress());
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    loadData();
  }, [loadData]);

  const handleToggleComplete = async (moduleId: string, completed: boolean) => {
    // Optimistic update
    setProgress((prev) => ({ ...prev, [moduleId]: completed }));

    if (backendOnline) {
      try {
        await updateProgress(moduleId, completed);
      } catch {
        // Backend failed; persist to localStorage as fallback
        setLocalModuleProgress(moduleId, completed);
      }
    } else {
      setLocalModuleProgress(moduleId, completed);
    }
  };

  const completedCount = modules.filter((m) => progress[m.id]).length;
  const percent =
    modules.length > 0
      ? Math.round((completedCount / modules.length) * 100)
      : 0;

  return (
    <div className="min-h-screen bg-gray-950">
      {/* Header */}
      <header className="sticky top-0 z-10 bg-gray-950/95 backdrop-blur-sm border-b border-gray-800/60">
        <div className="max-w-3xl mx-auto px-4 py-3 flex items-center justify-between gap-4">
          <div className="flex items-center gap-3">
            <div className="w-8 h-8 rounded-lg bg-blue-600 flex items-center justify-center">
              <BookOpen className="w-4 h-4 text-white" />
            </div>
            <div>
              <h1 className="text-sm font-semibold text-gray-100 leading-tight">
                MLOps Roadmap
              </h1>
              <p className="text-xs text-gray-500 leading-tight">.NET & Python</p>
            </div>
          </div>

          <div className="flex items-center gap-4">
            {/* Progress pill */}
            {!loading && modules.length > 0 && (
              <div className="flex items-center gap-2 bg-gray-900 border border-gray-700/50 rounded-full px-3 py-1.5">
                <div className="w-16 bg-gray-800 rounded-full h-1.5 overflow-hidden">
                  <div
                    className="h-full bg-gradient-to-r from-blue-600 to-emerald-500 rounded-full transition-all duration-500"
                    style={{ width: `${percent}%` }}
                  />
                </div>
                <span className="text-xs text-gray-400">
                  {completedCount}/{modules.length}
                </span>
              </div>
            )}

            {/* Backend status */}
            <div
              className={`flex items-center gap-1.5 text-xs ${
                backendOnline ? "text-emerald-500" : "text-yellow-600"
              }`}
              title={backendOnline ? "Backend connected" : "Using local storage"}
            >
              {backendOnline ? (
                <Wifi className="w-3.5 h-3.5" />
              ) : (
                <WifiOff className="w-3.5 h-3.5" />
              )}
              <span className="hidden sm:inline">
                {backendOnline ? "Online" : "Offline"}
              </span>
            </div>
          </div>
        </div>
      </header>

      {/* Main content */}
      <main>
        {loading ? (
          <div className="flex flex-col items-center justify-center min-h-[60vh] gap-4">
            <div className="w-10 h-10 rounded-full border-2 border-blue-500 border-t-transparent animate-spin" />
            <p className="text-gray-500 text-sm">Loading roadmap…</p>
          </div>
        ) : view.type === "roadmap" ? (
          <RoadmapView
            modules={modules}
            progress={progress}
            onSelectModule={(id) => setView({ type: "lesson", moduleId: id })}
          />
        ) : (
          <LessonView
            moduleId={view.moduleId}
            progress={progress}
            onBack={() => setView({ type: "roadmap" })}
            onToggleComplete={handleToggleComplete}
          />
        )}
      </main>
    </div>
  );
}
