import { Injectable, signal, effect } from '@angular/core';

export type ThemeMode = 'light' | 'dark' | 'auto';

@Injectable({
  providedIn: 'root'
})
export class ThemeService {
  public currentTheme = signal<ThemeMode>('auto');

  constructor() {
    const saved = localStorage.getItem('ef_theme') as ThemeMode;
    if (saved) {
      this.currentTheme.set(saved);
    }
    
    // Set up a side effect that automatically applies the theme when it changes
    effect(() => {
      this.applyTheme(this.currentTheme());
    });
  }

  setTheme(theme: ThemeMode) {
    localStorage.setItem('ef_theme', theme);
    this.currentTheme.set(theme);
  }

  private applyTheme(mode: ThemeMode) {
    const root = document.documentElement;
    root.classList.remove('dark-theme');

    if (mode === 'dark') {
      root.classList.add('dark-theme');
    } else if (mode === 'auto') {
      const systemPrefersDark = window.matchMedia('(prefers-color-scheme: dark)').matches;
      if (systemPrefersDark) {
        root.classList.add('dark-theme');
      }
    }
  }
}
