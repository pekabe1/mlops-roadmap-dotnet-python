"use client";

import { Award, BookOpen, CheckCircle, Circle } from "lucide-react";
import type { CourseModule, ProgressMap } from "@/types";
import ModuleCard from "./ModuleCard";

interface RoadmapViewProps {
  modules: CourseModule[];
  progress: ProgressMap;
  onSelectModule: (id: string) => void;
}

export default function RoadmapView({
  modules,
  progress,
  onSelectModule,
}: RoadmapViewProps) {
  const completedCount = modules.filter((m) => progress[m.id]).length;
  const percent =
    modules.length > 0
      ? Math.round((completedCount / modules.length) * 100)
      : 0;

  return (
    <div className="max-w-2xl mx-auto px-4 py-8">
      {/* Progress summary card */}
      <div className="bg-gray-900 border border-gray-700/50 rounded-2xl p-6 mb-8">
        <div className="flex items-center justify-between mb-4">
          <div>
            <h2 className="text-lg font-semibold text-gray-100">
              Your Progress
            </h2>
            <p className="text-gray-400 text-sm mt-0.5">
              {completedCount} of {modules.length} modules completed
            </p>
          </div>
          <div className="flex items-center gap-2">
            {percent === 100 ? (
              <Award className="w-8 h-8 text-yellow-400" />
            ) : (
              <span className="text-3xl font-bold text-blue-400">
                {percent}%
              </span>
            )}
          </div>
        </div>

        {/* Progress bar */}
        <div className="w-full bg-gray-800 rounded-full h-3 overflow-hidden">
          <div
            className="h-full rounded-full bg-gradient-to-r from-blue-600 to-emerald-500 transition-all duration-500"
            style={{ width: `${percent}%` }}
          />
        </div>

        {percent === 100 && (
          <div className="mt-4 flex items-center gap-2 text-yellow-400 text-sm font-medium">
            <Award className="w-4 h-4" />
            Congratulations! You&apos;ve completed the entire roadmap!
          </div>
        )}
      </div>

      {/* Legend */}
      <div className="flex items-center gap-6 mb-6 px-1">
        <span className="flex items-center gap-2 text-sm text-gray-500">
          <CheckCircle className="w-4 h-4 text-emerald-500" />
          Completed
        </span>
        <span className="flex items-center gap-2 text-sm text-gray-500">
          <Circle className="w-4 h-4 text-gray-600" />
          In progress
        </span>
        <span className="flex items-center gap-2 text-sm text-gray-500">
          <BookOpen className="w-4 h-4 text-gray-600" />
          Click to open
        </span>
      </div>

      {/* Module list */}
      <div>
        {modules.map((module, index) => (
          <ModuleCard
            key={module.id}
            module={module}
            isCompleted={!!progress[module.id]}
            progress={progress}
            onClick={() => onSelectModule(module.id)}
            isLast={index === modules.length - 1}
          />
        ))}
      </div>
    </div>
  );
}
