import axios from 'axios';
import { Submission } from '@/types/Submission';
import { SubmissionSearchResult } from '@/types/SubmissionSearchResult';
import { SubmissionFilter } from '@/types/SubmissionFilter';
import { SubmissionApi } from './SubmissionApi';
import { SUBMISSION_API_BASE } from '@/config';
    
export class HttpSubmissionApi implements SubmissionApi {
 
    async  createSubmission(formId: string, formName: string, payload: object): Promise<Submission> {
        try {
            const response = await axios.post<Submission>(
                `${SUBMISSION_API_BASE}/submissions`,
                {
                    formId,
                    formName,
                    payload: JSON.stringify(payload),
                }
            );

            return response.data;

        } catch (error: any) {
            console.error(error);
            throw new Error(error?.message || 'Failed to create submission');
        }
    }


    async getSubmissions(filter: SubmissionFilter): Promise<SubmissionSearchResult> {
        try {
            const response = await axios.get<SubmissionSearchResult>(
                `${SUBMISSION_API_BASE}/submissions`,
                {
                    params: filter
                }
            );
            return response.data;
            
        } catch (error: any) {
            console.error(error);
            throw new Error(error?.message || 'Failed to get sumbmissions');
        }
    }
}