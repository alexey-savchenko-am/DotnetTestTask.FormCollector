import {v4 as uuidv4} from 'uuid';
import { Submission } from '@/types/Submission';
import { SubmissionSearchResult } from '@/types/SubmissionSearchResult';
import { SubmissionFilter } from '@/types/SubmissionFilter';
import { SubmissionApi } from './SubmissionApi';

const submissions: Submission[] = [];

export class MockSubmissionApi implements SubmissionApi {
    async createSubmission(formId: string, formName:string, payload: object): Promise<Submission> {
        await new Promise(resolve => setTimeout(resolve, 200));

        const submission: Submission = {
            Id: uuidv4(),
            FormId: formId,
            FormName: formName,
            Payload: JSON.stringify(payload),
            CreatedOnUtc: new Date()
        }

        submissions.push(submission);

        return submission;
    }

    async getSubmissions(filter: SubmissionFilter): Promise<SubmissionSearchResult> {
        await new Promise(resolve => setTimeout(resolve, 200));

        let result: Submission[] = [...submissions];

        if(filter.FormId) {
            result = result.filter(s => s.FormId === filter.FormId);
        }

        if(filter.Query) {
            const keywords = filter.Query
                .split(' ')
                .map(k => k.trim().toLowerCase())
                .filter(k => k.length > 0);

            result = result.filter(s => {
                const payload = JSON.stringify(s.Payload).toLowerCase();
                return keywords.every(k => payload.includes(k));
            });
        }
        
        result = result.sort((a, b) => {
            return b.CreatedOnUtc.getTime() - a.CreatedOnUtc.getTime();
        });

        // skip + take
        const totalCount = result.length;

        const start = (filter.Page - 1) * filter.ItemsPerPage;
        const end = start + filter.ItemsPerPage;

        const paged = result.slice(start, end);

        return {
            Submissions: paged,
            TotalCount: totalCount
        }
    }
}