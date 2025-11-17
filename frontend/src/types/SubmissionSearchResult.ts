import { Submission } from '@/types/Submission';

export interface SubmissionSearchResult {
    Submissions: Submission[],
    TotalCount: number
}