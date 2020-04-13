import { Component, OnInit } from '@angular/core';
import { ProviderService } from '../_services/provider.service';
import { AlertifyService } from '../_services/alertify.service';

@Component({
  selector: 'app-provider',
  templateUrl: './provider.component.html',
  styleUrls: ['./provider.component.css'],
})
export class ProviderComponent implements OnInit {
  public provider: any = {
    ProviderAddress: {},
  };

  public usStates: any[] = [];

  constructor(
    private providerService: ProviderService,
    private alertify: AlertifyService
  ) {}

  ngOnInit() {
    this.getAllStates();
  }

  public registerProvider() {
    this.providerService.registerProvider(this.provider).subscribe(
      () => {
        this.alertify.success('Provider registered successfully');
      },
      (error) => {
        this.alertify.error('Failed to register Providers.');
      }
    );
  }

  private getAllStates() {
    this.providerService.getAllStates().subscribe(
      (data) => {
        this.usStates = data;
      },
      (error) => {
        this.alertify.error('Failed to register Providers');
      }
    );
  }
}
