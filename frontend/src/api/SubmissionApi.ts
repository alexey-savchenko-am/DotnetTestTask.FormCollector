import { Submission } from "@/types/Submission";
import { SubmissionSearchResult } from "@/types/SubmissionSearchResult";
import { SubmissionFilter } from "@/types/SubmissionFilter";

export interface SubmissionApi {
    createSubmission(formId: string, formName: string, payload: object): Promise<Submission>;
    getSubmissions(filter: SubmissionFilter): Promise<SubmissionSearchResult>;
}