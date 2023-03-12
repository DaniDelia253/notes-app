import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavbarComponent } from './navbar/navbar.component';
import { FormsModule } from '@angular/forms';
import { HomeComponent } from './home/home.component';
import { RegisterFormComponent } from './register-form/register-form.component';
import { NotesComponent } from './notes/notes.component';

@NgModule({
  declarations: [AppComponent, NavbarComponent, HomeComponent, RegisterFormComponent, NotesComponent],
  imports: [BrowserModule, AppRoutingModule, HttpClientModule, FormsModule],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
