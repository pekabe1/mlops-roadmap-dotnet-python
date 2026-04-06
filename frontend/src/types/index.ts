export interface CourseModule {
  id: string;
  title: string;
  emoji: string;
  description: string;
  order: number;
  tags: string[];
  tasks: string[];
}

export interface CourseModuleDetail extends CourseModule {
  content: string;
}

export interface ProgressRecord {
  moduleId: string;
  completed: boolean;
  updatedAt: string;
}

export type ProgressMap = Record<string, boolean>;
