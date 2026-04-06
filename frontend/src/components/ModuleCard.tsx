"use client";

import { CheckCircle, Circle, ChevronRight, Tag, BookOpen } from "lucide-react";
import type { CourseModule, ProgressMap } from "@/types";

interface ModuleCardProps {
  module: CourseModule;
  isCompleted: boolean;
  progress: ProgressMap;
  onClick: () => void;
  isLast: boolean;
}

export default function ModuleCard({
  module,
  isCompleted,
  onClick,
  isLast,
}: ModuleCardProps) {
  return (
    <div className="relative flex gap-4">
      {/* Connector line */}
      {!isLast && (
        <div
          className="absolute left-[19px] top-[44px] w-0.5 bg-gray-700"
          style={{ height: "calc(100% + 16px)" }}
        />
      )}

      {/* Step indicator */}
      <div className="flex-shrink-0 flex flex-col items-center">
        <div
          className={`w-10 h-10 rounded-full flex items-center justify-center text-sm font-bold border-2 transition-all ${
            isCompleted
              ? "bg-emerald-500 border-emerald-400 text-white"
              : "bg-gray-800 border-gray-600 text-gray-400"
          }`}
        >
          {isCompleted ? (
            <CheckCircle className="w-5 h-5" />
          ) : (
            <span>{module.order}</span>
          )}
        </div>
      </div>

      {/* Card content */}
      <button
        onClick={onClick}
        className={`flex-1 mb-4 text-left p-4 rounded-xl border transition-all duration-200 group hover:scale-[1.01] ${
          isCompleted
            ? "bg-emerald-950/30 border-emerald-800/50 hover:border-emerald-600"
            : "bg-gray-900 border-gray-700/50 hover:border-blue-500/60"
        }`}
      >
        <div className="flex items-start justify-between gap-3">
          <div className="flex-1 min-w-0">
            <div className="flex items-center gap-2 mb-1">
              <span className="text-xl">{module.emoji}</span>
              <h3
                className={`font-semibold text-base truncate ${
                  isCompleted ? "text-emerald-300" : "text-gray-100"
                }`}
              >
                {module.title}
              </h3>
            </div>
            <p className="text-gray-400 text-sm leading-relaxed line-clamp-2">
              {module.description}
            </p>

            <div className="flex flex-wrap items-center gap-3 mt-3">
              {/* Task count */}
              <span className="flex items-center gap-1 text-xs text-gray-500">
                <BookOpen className="w-3.5 h-3.5" />
                {module.tasks.length} tasks
              </span>

              {/* Tags */}
              <div className="flex flex-wrap gap-1">
                {module.tags.slice(0, 3).map((tag) => (
                  <span
                    key={tag}
                    className="flex items-center gap-0.5 px-2 py-0.5 rounded-full bg-gray-800 text-gray-400 text-xs border border-gray-700/50"
                  >
                    <Tag className="w-2.5 h-2.5" />
                    {tag}
                  </span>
                ))}
              </div>
            </div>
          </div>

          <ChevronRight
            className={`w-5 h-5 flex-shrink-0 mt-1 transition-transform group-hover:translate-x-1 ${
              isCompleted ? "text-emerald-500" : "text-gray-600"
            }`}
          />
        </div>

        {isCompleted && (
          <div className="mt-3 pt-3 border-t border-emerald-800/30 flex items-center gap-1.5 text-xs text-emerald-500">
            <CheckCircle className="w-3.5 h-3.5" />
            Module completed
          </div>
        )}
      </button>
    </div>
  );
}
