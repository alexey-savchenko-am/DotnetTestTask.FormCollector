import { SubmissionApi } from "./SubmissionApi";
import { MockSubmissionApi } from "./MockSubmissionApi";
import { HttpSubmissionApi } from "./HttpSubmissionApi";

export const submissionApi: SubmissionApi = new HttpSubmissionApi();