import { SubmissionApi } from "./SubmissionApi";
import { MockSubmissionApi } from "./MockSubmissionApi";

export const submissionApi: SubmissionApi = new MockSubmissionApi();