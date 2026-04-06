"use client";

import { useEffect, useState, useCallback } from "react";
import ReactMarkdown from "react-markdown";
import remarkGfm from "remark-gfm";
import rehypeHighlight from "rehype-highlight";
import "highlight.js/styles/github-dark.css";
import {
  ChevronLeft,
  CheckCircle,
  Circle,
  Tag,
  Code,
  CheckSquare,
} from "lucide-react";
import type { CourseModuleDetail, ProgressMap } from "@/types";
import { fetchModule } from "@/lib/api";

const TASK_STORAGE_PREFIX = "mlops-tasks-";

interface LessonViewProps {
  moduleId: string;
  progress: ProgressMap;
  onBack: () => void;
  onToggleComplete: (moduleId: string, completed: boolean) => void;
}

export default function LessonView({
  moduleId,
  progress,
  onBack,
  onToggleComplete,
}: LessonViewProps) {
  const [module, setModule] = useState<CourseModuleDetail | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [completedTasks, setCompletedTasks] = useState<Record<string, boolean>>(
    {}
  );

  const storageKey = `${TASK_STORAGE_PREFIX}${moduleId}`;

  const loadTaskProgress = useCallback(() => {
    try {
      const raw = localStorage.getItem(storageKey);
      return raw ? (JSON.parse(raw) as Record<string, boolean>) : {};
    } catch {
      return {};
    }
  }, [storageKey]);

  useEffect(() => {
    setLoading(true);
    setError(null);
    fetchModule(moduleId)
      .then((data) => {
        setModule(data);
        setCompletedTasks(loadTaskProgress());
      })
      .catch((err: unknown) => {
        const msg = err instanceof Error ? err.message : "Failed to load module";
        setError(msg);
      })
      .finally(() => setLoading(false));
  }, [moduleId, loadTaskProgress]);

  const toggleTask = (taskIndex: number) => {
    const key = `task-${taskIndex}`;
    const updated = { ...completedTasks, [key]: !completedTasks[key] };
    setCompletedTasks(updated);
    try {
      localStorage.setItem(storageKey, JSON.stringify(updated));
    } catch {
      // ignore
    }
  };

  const markAllComplete = () => {
    if (!module) return;
    const allDone: Record<string, boolean> = {};
    module.tasks.forEach((_, i) => {
      allDone[`task-${i}`] = true;
    });
    setCompletedTasks(allDone);
    try {
      localStorage.setItem(storageKey, JSON.stringify(allDone));
    } catch {
      // ignore
    }
    onToggleComplete(moduleId, true);
  };

  const isModuleCompleted = !!progress[moduleId];

  if (loading) {
    return (
      <div className="max-w-3xl mx-auto px-4 py-16 flex flex-col items-center gap-4">
        <div className="w-8 h-8 rounded-full border-2 border-blue-500 border-t-transparent animate-spin" />
        <p className="text-gray-500 text-sm">Loading lesson…</p>
      </div>
    );
  }

  if (error || !module) {
    return (
      <div className="max-w-3xl mx-auto px-4 py-16 text-center">
        <p className="text-red-400 mb-4">{error ?? "Module not found"}</p>
        <button
          onClick={onBack}
          className="px-4 py-2 bg-gray-800 rounded-lg text-sm text-gray-300 hover:bg-gray-700"
        >
          ← Back to roadmap
        </button>
      </div>
    );
  }

  const taskDoneCount = Object.values(completedTasks).filter(Boolean).length;

  return (
    <div className="max-w-3xl mx-auto px-4 py-8">
      {/* Back button */}
      <button
        onClick={onBack}
        className="flex items-center gap-1.5 text-gray-400 hover:text-gray-200 text-sm mb-6 transition-colors group"
      >
        <ChevronLeft className="w-4 h-4 group-hover:-translate-x-0.5 transition-transform" />
        Back to roadmap
      </button>

      {/* Module header */}
      <div
        className={`rounded-2xl p-6 mb-8 border ${
          isModuleCompleted
            ? "bg-emerald-950/30 border-emerald-800/40"
            : "bg-gray-900 border-gray-700/50"
        }`}
      >
        <div className="flex items-start justify-between gap-4">
          <div className="flex-1 min-w-0">
            <div className="flex items-center gap-3 mb-2">
              <span className="text-3xl">{module.emoji}</span>
              <div>
                <div className="text-xs text-gray-500 mb-1">
                  Module {module.order}
                </div>
                <h1 className="text-xl font-bold text-gray-100">
                  {module.title}
                </h1>
              </div>
            </div>
            <p className="text-gray-400 text-sm leading-relaxed">
              {module.description}
            </p>

            {/* Tags */}
            <div className="flex flex-wrap gap-1.5 mt-3">
              {module.tags.map((tag) => (
                <span
                  key={tag}
                  className="flex items-center gap-1 px-2 py-0.5 rounded-full bg-gray-800 text-gray-400 text-xs border border-gray-700/50"
                >
                  <Tag className="w-2.5 h-2.5" />
                  {tag}
                </span>
              ))}
            </div>
          </div>

          {/* Complete toggle */}
          <button
            onClick={() => onToggleComplete(moduleId, !isModuleCompleted)}
            className={`flex-shrink-0 flex items-center gap-2 px-4 py-2 rounded-xl text-sm font-medium transition-all ${
              isModuleCompleted
                ? "bg-emerald-900/50 text-emerald-300 border border-emerald-700 hover:bg-emerald-900"
                : "bg-gray-800 text-gray-400 border border-gray-700 hover:border-blue-500 hover:text-blue-400"
            }`}
          >
            {isModuleCompleted ? (
              <>
                <CheckCircle className="w-4 h-4" />
                Completed
              </>
            ) : (
              <>
                <Circle className="w-4 h-4" />
                Mark complete
              </>
            )}
          </button>
        </div>
      </div>

      {/* Task checklist */}
      <div className="bg-gray-900 border border-gray-700/50 rounded-2xl p-6 mb-8">
        <div className="flex items-center justify-between mb-4">
          <div className="flex items-center gap-2">
            <CheckSquare className="w-4 h-4 text-blue-400" />
            <h2 className="font-semibold text-gray-200 text-sm">
              Learning Tasks
            </h2>
            <span className="text-xs text-gray-500 bg-gray-800 px-2 py-0.5 rounded-full">
              {taskDoneCount}/{module.tasks.length}
            </span>
          </div>
          {taskDoneCount < module.tasks.length && (
            <button
              onClick={markAllComplete}
              className="text-xs text-blue-400 hover:text-blue-300 transition-colors"
            >
              Mark all complete
            </button>
          )}
        </div>

        {/* Task progress bar */}
        <div className="w-full bg-gray-800 rounded-full h-1.5 mb-4 overflow-hidden">
          <div
            className="h-full rounded-full bg-gradient-to-r from-blue-600 to-emerald-500 transition-all duration-300"
            style={{
              width: `${module.tasks.length > 0 ? (taskDoneCount / module.tasks.length) * 100 : 0}%`,
            }}
          />
        </div>

        <ul className="space-y-2">
          {module.tasks.map((task, i) => {
            const done = !!completedTasks[`task-${i}`];
            return (
              <li key={i}>
                <button
                  onClick={() => toggleTask(i)}
                  className={`w-full flex items-center gap-3 p-3 rounded-lg text-left transition-all ${
                    done
                      ? "bg-emerald-950/30 text-emerald-300"
                      : "bg-gray-800/50 text-gray-300 hover:bg-gray-800"
                  }`}
                >
                  {done ? (
                    <CheckCircle className="w-4 h-4 text-emerald-500 flex-shrink-0" />
                  ) : (
                    <Circle className="w-4 h-4 text-gray-600 flex-shrink-0" />
                  )}
                  <span
                    className={`text-sm ${done ? "line-through opacity-60" : ""}`}
                  >
                    {task}
                  </span>
                </button>
              </li>
            );
          })}
        </ul>
      </div>

      {/* Lesson content */}
      <div className="bg-gray-900 border border-gray-700/50 rounded-2xl p-6">
        <div className="flex items-center gap-2 mb-6 pb-4 border-b border-gray-800">
          <Code className="w-4 h-4 text-blue-400" />
          <h2 className="font-semibold text-gray-200 text-sm">Lesson Content</h2>
        </div>

        <div className="prose prose-invert prose-sm max-w-none
          prose-headings:text-gray-100
          prose-h1:text-xl prose-h1:font-bold prose-h1:mb-4
          prose-h2:text-lg prose-h2:font-semibold prose-h2:mt-8 prose-h2:mb-3
          prose-h3:text-base prose-h3:font-medium prose-h3:mt-6 prose-h3:mb-2
          prose-p:text-gray-300 prose-p:leading-relaxed prose-p:my-3
          prose-a:text-blue-400 prose-a:no-underline hover:prose-a:underline
          prose-strong:text-gray-200 prose-strong:font-semibold
          prose-code:text-blue-300 prose-code:bg-gray-800 prose-code:px-1.5 prose-code:py-0.5 prose-code:rounded prose-code:text-xs prose-code:font-mono
          prose-pre:bg-gray-950 prose-pre:border prose-pre:border-gray-700/50 prose-pre:rounded-xl prose-pre:p-0
          prose-pre:code:bg-transparent prose-pre:code:text-sm prose-pre:code:p-4 prose-pre:code:block
          prose-ul:text-gray-300 prose-ul:my-3
          prose-ol:text-gray-300
          prose-li:my-1
          prose-blockquote:border-blue-500 prose-blockquote:text-gray-400
          prose-table:text-gray-300
          prose-thead:border-gray-700
          prose-tbody:border-gray-800
          prose-th:text-gray-200 prose-th:font-semibold prose-th:bg-gray-800/50
          prose-td:border-gray-800
          prose-hr:border-gray-800">
          <ReactMarkdown remarkPlugins={[remarkGfm]} rehypePlugins={[rehypeHighlight]}>
            {module.content}
          </ReactMarkdown>
        </div>
      </div>

      {/* Bottom back button */}
      <div className="mt-8 flex justify-center">
        <button
          onClick={onBack}
          className="flex items-center gap-1.5 px-6 py-2.5 bg-gray-800 hover:bg-gray-700 rounded-xl text-sm text-gray-300 transition-colors"
        >
          <ChevronLeft className="w-4 h-4" />
          Back to roadmap
        </button>
      </div>
    </div>
  );
}
