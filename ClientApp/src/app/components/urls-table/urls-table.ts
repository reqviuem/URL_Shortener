import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { ApiService, ShortUrl } from '../../services/api';
import { AuthService } from '../../services/auth';

@Component({
  selector: 'app-urls-table',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './urls-table.html',
  styleUrl: './urls-table.css'
})
export class UrlsTable implements OnInit {
  urls: ShortUrl[] = [];
  newUrl: string = '';
  errorMessage: string = '';

  isLoggedIn: boolean = false;
  currentUsername: string | null = '';

  constructor(
    private apiService: ApiService,
    public authService: AuthService
  ) {}

  ngOnInit() {
    this.loadUrls();

    this.authService.isLoggedIn$.subscribe(status => {
      this.isLoggedIn = status;
      this.currentUsername = this.authService.getUsername();
    });
  }

  loadUrls() {
    this.apiService.getAllUrls().subscribe({
      next: (data) => this.urls = data,
      error: (err) => console.error('Failed to load URLs', err)
    });
  }

  addUrl() {
    if (!this.newUrl) return;
    this.errorMessage = '';

    this.apiService.addUrl(this.newUrl).subscribe({
      next: (createdUrl) => {
        this.urls.push(createdUrl);
        this.newUrl = '';
      },
      error: (err) => {
        this.errorMessage = err.error?.message || 'Failed to add URL';
      }
    });
  }

  deleteUrl(id: number) {
    if (!confirm('Are you sure you want to delete this URL?')) return;

    this.apiService.deleteUrl(id).subscribe({
      next: () => {
        this.urls = this.urls.filter(u => u.id !== id);
      },
      error: (err) => alert('Failed to delete URL')
    });
  }

  // Helper to check if a user is allowed to see the Delete button
  // Admins can delete all; ordinary users can only delete their own.
  canDelete(url: ShortUrl): boolean {
    if (!this.isLoggedIn) return false;

    // In a real app, you'd check a "Role" claim from the JWT here.
    // For now, we check if they are the creator (or you can add role logic later).
    return this.currentUsername === url.createdBy || this.currentUsername === 'admin';
  }
}
