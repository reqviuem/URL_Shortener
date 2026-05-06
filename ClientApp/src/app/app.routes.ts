import { Routes } from '@angular/router';
import { Login } from './components/login/login';
import { UrlsTable } from './components/urls-table/urls-table';
import { UrlInfo } from './components/url-info/url-info';
import { authGuard } from './guards/auth-guard';

export const routes: Routes = [
  { path: '', redirectTo: 'urls', pathMatch: 'full' },
  { path: 'login', component: Login },
  { path: 'urls', component: UrlsTable },
  { path: 'url/:id', component: UrlInfo, canActivate: [authGuard] },
  { path: '**', redirectTo: 'urls' }
];
