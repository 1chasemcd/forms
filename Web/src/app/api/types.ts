import {
  Button,
  CheckBoxInput,
  CombinedView,
  CurrencyInput,
  DataView,
  DateInput,
  NumericInput,
  RepositoryGridView,
  StaticTextField,
  SubPropertyGridView,
  TextAreaInput,
  TextInput,
  TimeInput,
} from './api.g';

export type FormDataView = DataView & { $type: 'dataview' };
export type FormCombinedView = CombinedView & { $type: 'combinedview' };
export type FormSubPropertyGridView = SubPropertyGridView & {
  $type: 'subpropertygridview';
};
export type FormRepositoryGridView = RepositoryGridView & {
  $type: 'repositorygridview';
};

export type FormView =
  | FormDataView
  | FormCombinedView
  | FormSubPropertyGridView
  | FormRepositoryGridView;

export type FormCheckBox = CheckBoxInput & { $type: 'checkboxinput' };
export type FormText = TextInput & { $type: 'textinput' };
export type FormTextArea = TextAreaInput & { $type: 'textarea' };
export type FormCurrency = CurrencyInput & { $type: 'currencyinput' };
export type FormNumeric = NumericInput & { $type: 'numericinput' };
export type FormDate = DateInput & { $type: 'dateinput' };
export type FormTime = TimeInput & { $type: 'timeinput' };
export type FormButton = Button & { $type: 'button' };
export type FormStaticText = StaticTextField & { $type: 'statictextfield' };

export type FormField =
  | FormCheckBox
  | FormText
  | FormTextArea
  | FormCurrency
  | FormNumeric
  | FormDate
  | FormTime
  | FormButton
  | FormStaticText;
