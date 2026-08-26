# Photos

The landing page expects the files listed below. They are **not** committed,
because photographs of real students and staff are someone's likeness and
someone's copyright - shipping stock people as "our students" in a public repo
is a licensing problem, not a design decision.

Drop your own in here, or download free-licensed ones (Unsplash / Pexels both
allow commercial use without attribution) and rename them to match:

| File               | What it should show                    | Suggested crop |
| ------------------ | -------------------------------------- | -------------- |
| `campus.jpg`       | Campus building or courtyard           | 1600 x 900     |
| `hero-study.jpg`   | Students working together at a table   | 1200 x 1500    |
| `hero-lecture.jpg` | A lecture or lab in progress           | 1200 x 800     |
| `teacher-1.jpg`    | Portrait, teacher                      | 600 x 600      |
| `teacher-2.jpg`    | Portrait, teacher                      | 600 x 600      |
| `student-1.jpg`    | Portrait, student                      | 600 x 600      |
| `student-2.jpg`    | Portrait, student                      | 600 x 600      |
| `student-3.jpg`    | Portrait, student                      | 600 x 600      |

Until a file exists, the `<Photo>` component draws a designed placeholder in
its place - a tinted panel with the subject's initials - so the layout never
shows a broken image icon. Nothing breaks if you skip this step.

Keep each file under ~300 KB. `squoosh.app` will do it in the browser.
