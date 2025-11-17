import { Submission } from "@/types/Submission";
import { SubmissionSearchResult } from "@/types/SubmissionSearchResult";
import { SubmissionFilter } from "@/types/SubmissionFilter";

export interface SubmissionApi {
    createClient(payload: object): Promise<Submission>;
    getSubmissions(filter: SubmissionFilter): Promise<SubmissionSearchResult>;
}