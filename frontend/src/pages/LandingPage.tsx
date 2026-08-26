import { Link } from 'react-router-dom';
import { ArrowRight, ArrowUpRight } from 'lucide-react';

import { Wordmark } from '../components/AppShell';
import { Photo } from '../components/Photo';
import { Button } from '../components/ui';

const departments = [
  'Computer Science',
  'Business Administration',
  'Electrical Engineering',
  'Pharmacy',
  'English',
  'Economics',
  'Civil Engineering',
  'Law',
];

const roles = [
  {
    index: '01',
    name: 'Teachers',
    line: 'Set the work, watch it arrive, mark it.',
    body:
      'Write an assignment, attach the brief, and publish it to a whole academic year in one action. Submissions land in a single queue with the time each one arrived, late work already flagged. Enter marks and feedback against the total, and the student is told straight away.',
    points: ['Draft before you publish', 'One queue per assignment', 'Marks out of the total'],
    photo: '/images/hero-lecture.jpg',
    alt: 'A teacher leading a class',
  },
  {
    index: '02',
    name: 'Students',
    line: 'Know what is due, and when it stops counting.',
    body:
      'Every assignment for your year is on one page, ordered by deadline and colour-coded: open, closing soon, or inside the late window. Hand in a note, a file, or both. Once it is marked, the grade and the feedback sit next to what you submitted.',
    points: ['Deadlines you can see at a glance', 'Notes and file attachments', 'Feedback in the same place'],
    photo: '/images/hero-study.jpg',
    alt: 'Students working together',
  },
  {
    index: '03',
    name: 'Administrators',
    line: 'Departments, courses, batches, enrolments.',
    body:
      'Set up the academic structure the rest of it hangs from: departments and courses, subjects mapped to courses, batches split into academic years, teachers assigned to subjects, students enrolled in years. A checklist shows which step is still missing.',
    points: ['Courses and subjects', 'Teacher assignments', 'Student enrolment'],
    photo: '/images/campus.jpg',
    alt: 'Campus building',
  },
];

const flow = [
  {
    step: 'Publish',
    text: 'A teacher publishes an assignment with a due date and, if they choose, a late window.',
  },
  {
    step: 'Notify',
    text: 'Every student enrolled in that academic year is told, with the subject and the deadline.',
  },
  {
    step: 'Submit',
    text: 'Students hand in notes and a file. Anything after the deadline is recorded as late.',
  },
  {
    step: 'Grade',
    text: 'The teacher marks it out of the total and writes feedback. The student is notified.',
  },
];

const people = [
  { photo: '/images/teacher-1.jpg', name: 'Faculty', role: 'Computer Science' },
  { photo: '/images/student-1.jpg', name: 'Student', role: '2nd year, CSE' },
  { photo: '/images/teacher-2.jpg', name: 'Faculty', role: 'Business Admin' },
  { photo: '/images/student-2.jpg', name: 'Student', role: '3rd year, BBA' },
  { photo: '/images/student-3.jpg', name: 'Student', role: '1st year, CSE' },
];

export default function LandingPage() {
  return (
    <div className="min-h-screen bg-paper">
      <TopBar />
      <Hero />
      <Marquee />
      <Flow />
      <Roles />
      <People />
      <CallToAction />
      <Footer />
    </div>
  );
}

/* -------------------------------------------------------------------------- */

function TopBar() {
  return (
    <header className="sticky top-0 z-40 border-b border-white/10 bg-ink">
      <div className="mx-auto flex max-w-6xl items-center gap-6 px-4 py-3 sm:px-6">
        <Wordmark />

        <span
          aria-hidden
          className="hidden h-4 w-px bg-white/15 md:block"
        />

        <p className="hidden font-mono text-[11px] uppercase tracking-[0.16em] text-paper/35 md:block">
          Assignments &amp; submissions
        </p>

        <nav className="ml-auto hidden items-center gap-6 md:flex">
          {[
            ['How it works', '#flow'],
            ['Roles', '#roles'],
            ['People', '#people'],
          ].map(([label, href]) => (
            <a
              key={href}
              href={href}
              className="text-sm text-paper/60 transition-colors hover:text-paper"
            >
              {label}
            </a>
          ))}
        </nav>

        <Link to="/login" className="ml-auto md:ml-0">
          <Button variant="accent" size="sm">
            Sign in
            <ArrowRight className="h-3.5 w-3.5" />
          </Button>
        </Link>
      </div>
    </header>
  );
}

function Hero() {
  return (
    <section className="relative overflow-hidden border-b border-white/10 bg-ink py-14 sm:py-16">
      <div className="absolute inset-0 z-0">
        <img
          src="/campus-hero-bg.jpg"
          alt=""
          className="h-full w-full object-cover blur-[4px] scale-105"
        />
        <div className="absolute inset-0 bg-zinc-950/85" />
      </div>

      <div className="relative z-10 mx-auto max-w-6xl px-4 sm:px-6">
        <div className="grid gap-12 lg:grid-cols-[1.05fr_0.95fr] lg:items-center">
          <div>
            <p className="ledger-index text-acid">Assignment &amp; submission system</p>

            <h1 className="mt-4 font-display text-[2.5rem] font-semibold leading-[1.05] text-paper sm:text-[3.4rem]">
              Every deadline,
              <br />
              <span className="italic text-acid">every submission,</span>
              <br />
              one record.
            </h1>

            <p className="mt-6 max-w-lg text-base leading-relaxed text-paper/60">
              EduSphere is where a college sets work and gets it back. Teachers
              publish assignments and mark what comes in. Students see what is
              due and hand it in. Every submission keeps its timestamp, its
              version, and its grade in one place.
            </p>

            <div className="mt-8 flex flex-wrap items-center gap-3">
              <Link to="/login">
                <Button variant="accent">
                  Open the demo
                  <ArrowRight className="h-4 w-4" />
                </Button>
              </Link>

              <a href="#flow">
                <Button
                  variant="secondary"
                  className="border-paper/25 text-paper hover:border-paper hover:bg-white/5"
                >
                  How it works
                </Button>
              </a>
            </div>

            {/* Fills the space under the CTA that would otherwise sit empty,
                and says something concrete while doing it. */}
            <dl className="mt-10 grid max-w-lg grid-cols-3 gap-px overflow-hidden rounded-card border border-white/10 bg-white/10">
              {[
                ['3', 'roles'],
                ['1', 'record per submission'],
                ['0', 'emailed zip files'],
              ].map(([value, label]) => (
                <div key={label} className="bg-ink px-4 py-4">
                  <dt className="font-display text-3xl font-semibold text-paper">
                    {value}
                  </dt>
                  <dd className="mt-1 font-mono text-[10px] uppercase leading-tight tracking-[0.12em] text-paper/40">
                    {label}
                  </dd>
                </div>
              ))}
            </dl>
          </div>

          {/* Overlapping photo pair, deliberately off-grid. */}
          <div className="relative hidden lg:block">
            <Photo
              src="/images/hero-study.jpg"
              alt="Students working together on an assignment"
              label="Study Group"
              ratio="aspect-[4/5]"
              className="w-[76%] border border-white/10"
            />
            <Photo
              src="/images/hero-lecture.jpg"
              alt="A lecture in progress"
              label="Lecture Hall"
              ratio="aspect-[4/3]"
              className="absolute -bottom-6 right-0 w-[56%] border-4 border-ink"
            />
            <span className="absolute -left-3 top-10 rotate-[-90deg] font-mono text-[10px] uppercase tracking-[0.3em] text-paper/25">
              Est. record
            </span>
          </div>
        </div>
      </div>
    </section>
  );
}

function Marquee() {
  return (
    <div className="overflow-hidden border-b border-rule bg-acid py-2.5">
      <div className="flex w-max animate-marquee gap-8">
        {[...departments, ...departments].map((item, index) => (
          <span
            key={`${item}-${index}`}
            className="flex items-center gap-8 font-mono text-xs uppercase tracking-[0.18em] text-ink/80"
          >
            {item}
            <span aria-hidden className="h-1 w-1 rounded-full bg-ink/40" />
          </span>
        ))}
      </div>
    </div>
  );
}

function Flow() {
  return (
    <section id="flow" className="mx-auto max-w-6xl px-4 py-20 sm:px-6">
      <div className="max-w-xl">
        <p className="ledger-index">Section one</p>
        <h2 className="mt-2 font-display text-3xl font-semibold text-ink sm:text-4xl">
          One assignment, start to finish.
        </h2>
        <p className="mt-3 text-body-muted">
          The same four steps every time, whether it is a lab report or a term
          paper.
        </p>
      </div>

      <ol className="mt-10 grid gap-px overflow-hidden rounded-card border border-ink bg-ink sm:grid-cols-2 lg:grid-cols-4">
        {flow.map((item, index) => (
          <li key={item.step} className="bg-paper px-5 py-6">
            <span className="font-mono text-[11px] tracking-[0.2em] text-body-faint">
              {String(index + 1).padStart(2, '0')}
            </span>
            <h3 className="mt-2 font-display text-xl font-semibold text-ink">
              {item.step}
            </h3>
            <p className="mt-2 text-sm leading-relaxed text-body-muted">
              {item.text}
            </p>
          </li>
        ))}
      </ol>
    </section>
  );
}

function Roles() {
  return (
    <section id="roles" className="border-t border-rule bg-paper-warm py-20">
      <div className="mx-auto max-w-6xl px-4 sm:px-6">
        <div className="max-w-xl">
          <p className="ledger-index">Section two</p>
          <h2 className="mt-2 font-display text-3xl font-semibold text-ink sm:text-4xl">
            Three people, one workflow.
          </h2>
        </div>

        <div className="mt-12 space-y-16">
          {roles.map((role, index) => (
            <article
              key={role.index}
              className={`grid items-center gap-8 lg:grid-cols-2 ${
                index % 2 === 1 ? 'lg:[&>figure]:order-last' : ''
              }`}
            >
              <figure className="relative">
                <Photo
                  src={role.photo}
                  alt={role.alt}
                  label={role.name}
                  ratio="aspect-[16/11]"
                  className="border border-rule"
                />
                <figcaption className="absolute -bottom-3 left-4 rounded-full border border-ink bg-paper px-3 py-1 font-mono text-[11px] uppercase tracking-[0.16em] text-ink">
                  {role.index} / {role.name}
                </figcaption>
              </figure>

              <div>
                <h3 className="font-display text-2xl font-semibold text-ink">
                  {role.line}
                </h3>
                <p className="mt-3 max-w-md leading-relaxed text-body-muted">
                  {role.body}
                </p>

                <ul className="mt-5 space-y-2">
                  {role.points.map((point) => (
                    <li
                      key={point}
                      className="flex items-center gap-2.5 text-sm text-ink"
                    >
                      <span
                        aria-hidden
                        className="h-1.5 w-1.5 rounded-full bg-acid-deep"
                      />
                      {point}
                    </li>
                  ))}
                </ul>
              </div>
            </article>
          ))}
        </div>
      </div>
    </section>
  );
}

function People() {
  return (
    <section id="people" className="border-y border-rule py-20">
      <div className="mx-auto max-w-6xl px-4 sm:px-6">
        <div className="flex flex-wrap items-end justify-between gap-4">
          <div className="max-w-lg">
            <p className="ledger-index">Section three</p>
            <h2 className="mt-2 font-display text-3xl font-semibold text-ink sm:text-4xl">
              Built around the people who use it.
            </h2>
          </div>

          <p className="max-w-xs text-sm text-body-muted">
            A teacher only reaches their own assignments, a student only their
            own submissions. That is enforced on the server, not hidden in the
            interface.
          </p>
        </div>

        <ul className="mt-12 grid grid-cols-2 gap-4 sm:grid-cols-3 lg:grid-cols-5">
          {people.map((person, index) => (
            <li key={index} className="group">
              <Photo
                src={person.photo}
                alt={`${person.name}, ${person.role}`}
                label={person.name}
                ratio="aspect-square"
                rounded="rounded-full"
                className="border border-rule transition-transform duration-300 group-hover:-translate-y-1"
              />
              <p className="mt-3 text-center text-sm font-medium text-ink">
                {person.name}
              </p>
              <p className="text-center font-mono text-[11px] text-body-faint">
                {person.role}
              </p>
            </li>
          ))}
        </ul>
      </div>
    </section>
  );
}

function CallToAction() {
  return (
    <section className="mx-auto max-w-6xl px-4 py-20 sm:px-6">
      <div className="relative overflow-hidden rounded-card border border-ink bg-ink px-6 py-14 text-center sm:px-12">
        <div
          aria-hidden
          className="absolute inset-0 opacity-[0.07]"
          style={{
            backgroundImage:
              'repeating-linear-gradient(45deg, transparent 0 14px, #C8FF4D 14px 15px)',
          }}
        />

        <div className="relative">
          <h2 className="mx-auto max-w-2xl font-display text-3xl font-semibold text-paper sm:text-4xl">
            Sign in and set your first assignment.
          </h2>
          <p className="mx-auto mt-4 max-w-md text-paper/60">
            Three demo accounts, already loaded with a course, a batch and a few
            submissions. The account picker is on the sign-in page.
          </p>

          <Link to="/login" className="mt-8 inline-block">
            <Button variant="accent">
              Open the demo
              <ArrowUpRight className="h-4 w-4" />
            </Button>
          </Link>
        </div>
      </div>
    </section>
  );
}

function Footer() {
  return (
    <footer className="border-t border-rule">
      <div className="mx-auto flex max-w-6xl flex-wrap items-center justify-between gap-4 px-4 py-8 sm:px-6">
        <Wordmark tone="dark" />
        <p className="font-mono text-xs text-body-faint">
          Assignment &amp; submission management
        </p>
      </div>
    </footer>
  );
}
