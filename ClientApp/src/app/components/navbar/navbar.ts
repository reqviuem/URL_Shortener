import { Component } from '@angular/core';
import { RouterLink, Router  } from '@angular/router';
import { AsyncPipe, NgIf } from '@angular/common';
import { AuthService } from '../../services/auth';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [RouterLink, AsyncPipe, NgIf],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css'
})
export class NavbarComponent {
  constructor(public authService: AuthService, private router: Router) {}

  goToAbout() {
    const token = this.authService.getToken();

    if (token) {
      window.location.href = `http://localhost:5068/About?access_token=${token}`;
    } else {
      window.location.href = `http://localhost:5068/About`;
    }
  }

  onLogout() {
    this.authService.logout();
    this.router.navigate(['/urls']);
  }
}
