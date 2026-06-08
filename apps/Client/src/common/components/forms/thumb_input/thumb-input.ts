import { RefDomT } from '@/common/types/dom';
import { UseFormFieldImg } from '@/core/directives/use_form_field_img';
import { ChangeDetectionStrategy, Component, OnInit, Signal, ViewChild } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-thumb-input',
  imports: [ReactiveFormsModule],
  templateUrl: './thumb-input.html',
  styleUrl: './thumb-input.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ThumbInput extends UseFormFieldImg implements OnInit {
  @ViewChild('fileInput')
  private fileInput: RefDomT;

  public openFilePicker(): void {
    this.fileInput?.nativeElement?.click();
  }

  ngOnInit(): void {
    this.setupField();
  }
}
