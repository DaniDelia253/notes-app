import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register-form',
  templateUrl: './register-form.component.html',
  styleUrls: ['./register-form.component.css'],
})
export class RegisterFormComponent implements OnInit {
  @Output() onCancelRegisterMode = new EventEmitter();
  model: any = {};

  constructor(private accountService: AccountService) {}

  ngOnInit(): void {}

  register() {
    this.accountService.register(this.model).subscribe({
      next: (data) => {
        console.log(data);
        //send cancel event to set RegistarMode back to false in parent
      },
      error: (err) => console.log(err),
    });
  }

  cancelRegistration() {
    this.onCancelRegisterMode.emit(false);
  }
}
