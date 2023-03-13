import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Note } from 'src/app/_models/Note';

@Component({
  selector: 'app-all-notes-list',
  templateUrl: './all-notes-list.component.html',
  styleUrls: ['./all-notes-list.component.css'],
})
export class AllNotesListComponent implements OnInit {
  @Input() notes: Note[] = [];
  @Output() onNoteSelected = new EventEmitter();
  constructor() {}

  ngOnInit(): void {}

  onSelectNote(event: any) {
    let noteId = event.target.id;
    if (event.target.id === '') {
      noteId = event.target.parentElement.id;
    }
    this.onNoteSelected.emit(noteId);
  }
}
