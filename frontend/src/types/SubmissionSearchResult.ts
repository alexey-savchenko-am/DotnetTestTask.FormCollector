import { Submission } from '@/types/Submission';

export interface SubmissionSearchResult {
    submissions: Submission[],
    totalCount: number
}