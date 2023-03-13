import { HttpClient } from '@angular/common/http';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { UntypedFormArray } from '@angular/forms';
import { Note } from '../_models/Note';

@Component({
  selector: 'app-notes',
  templateUrl: './notes.component.html',
  styleUrls: ['./notes.component.css'],
})
export class NotesComponent implements OnInit {
  @Output() currentNoteUpdated = new EventEmitter();
  notes: Note[] = [];
  currentNote: Note | undefined;

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.http.get('http://localhost:5273/api/note/').subscribe({
      next: (data) => {
        this.notes = Object.values(data);
      },
      error: (error) => console.log(error),
    });
  }

  onNoteSelected(noteId: number) {
    this.currentNote = this.notes.find((note) => note.note_id === +noteId);
    this.currentNoteUpdated.emit(this.currentNote);
  }
}
