import { Component, Input, OnInit } from '@angular/core';
import { Note } from 'src/app/_models/Note';

@Component({
  selector: 'app-current-note-display',
  templateUrl: './current-note-display.component.html',
  styleUrls: ['./current-note-display.component.css'],
})
export class CurrentNoteDisplayComponent implements OnInit {
  @Input() currentNote: Note | undefined = undefined;

  constructor() {}

  ngOnInit(): void {}
}
