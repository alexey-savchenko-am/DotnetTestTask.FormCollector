import {v4 as uuidv4} from 'uuid';
import { Submission } from '@/types/Submission';
import { SubmissionSearchResult } from '@/types/SubmissionSearchResult';
import { SubmissionFilter } from '@/types/SubmissionFilter';
import { SubmissionApi } from './SubmissionApi';

export class MockSubmissionApi implements SubmissionApi {

    _submissions: Submission[] = [];

    async createSubmission(formId: string, formName:string, payload: object): Promise<Submission> {
        await new Promise(resolve => setTimeout(resolve, 200));

        const submission: Submission = {
            id: uuidv4(),
            formId: formId,
            formName: formName,
            payload: JSON.stringify(payload),
            createdOnUtc: new Date()
        }

        this._submissions.push(submission);

        return submission;
    }

    async getSubmissions(filter: SubmissionFilter): Promise<SubmissionSearchResult> {
        await new Promise(resolve => setTimeout(resolve, 200));

        let result: Submission[] = [...this._submissions];

        console.log("submissions: ", result);

        if(filter.query) {
            const keywords = filter.query
                .split(' ')
                .map(k => k.trim().toLowerCase())
                .filter(k => k.length > 0);

            result = result.filter(s => {
                const payload = JSON.stringify(s.payload).toLowerCase();
                return keywords.every(k => payload.includes(k) || s.formId.includes(k));
            });
        }
        
        result = result.sort((a, b) => {
            return b.createdOnUtc.getTime() - a.createdOnUtc.getTime();
        });

        // skip + take
        const totalCount = result.length;

        const start = (filter.page - 1) * filter.itemsPerPage;
        const end = start + filter.itemsPerPage;

        const paged = result.slice(start, end);

        return {
            submissions: paged,
            totalCount: totalCount
        }
    }
}