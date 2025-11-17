import {v4 as uuidv4} from 'uuid';
import { Submission } from '../types/Submission';
import { SubmissionSearchResult } from '../types/SubmissionSearchResult';

const submissions: Submission[] = [];

export class SubmissionApi {
    static async createClient(payload: object): Promise<Submission> {
        await new Promise(resolve => setTimeout(resolve, 200));

        const submission: Submission = {
            Id: uuidv4(),
            FormId: "client-form",
            FormName: "Create New Client",
            Payload: JSON.stringify(payload),
            CreatedOnUtc: new Date().toISOString()
        }

        submissions.push(submission);

        return submission;
    }

    static async getSubmissions(): Promise<SubmissionSearchResult> {
        await new Promise(resolve => setTimeout(resolve, 200));
        
        const result: SubmissionSearchResult = {
            Submissions: submissions,
            TotalCount: submissions.length
        }

        return result;
    }
}