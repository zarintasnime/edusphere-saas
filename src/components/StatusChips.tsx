import { Badge } from './ui';
import { deadlineInfo } from '../lib/format';
import type { Assignment, SubmissionStatus } from '../lib/types';

/** Colour-coded state of an assignment's due date. */
export function DeadlineChip({ assignment }: { assignment: Assignment }) {
  const { state, label } = deadlineInfo(assignment);

  const tone =
    state === 'open' ? 'open' : state === 'soon' ? 'soon' : state === 'over' ? 'over' : 'neutral';

  return <Badge tone={tone}>{label}</Badge>;
}

const submissionTone: Record<SubmissionStatus, 'neutral' | 'ink' | 'open' | 'soon'> = {
  Submitted: 'ink',
  UnderReview: 'soon',
  Reviewed: 'open',
  Returned: 'neutral',
};

const submissionLabel: Record<SubmissionStatus, string> = {
  Submitted: 'Submitted',
  UnderReview: 'Under review',
  Reviewed: 'Graded',
  Returned: 'Returned',
};

export function SubmissionChip({ status }: { status: SubmissionStatus }) {
  return <Badge tone={submissionTone[status]}>{submissionLabel[status]}</Badge>;
}

export function AssignmentStatusChip({ assignment }: { assignment: Assignment }) {
  const tone = assignment.assignmentStatus === 'Published' ? 'ink' : 'neutral';
  return <Badge tone={tone}>{assignment.assignmentStatus}</Badge>;
}
