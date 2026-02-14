// @ts-check
import pkg from '@eslint/js';
const { configs } = pkg;
import { defineConfig } from 'eslint/config';
import { configs as tsconfigs } from 'typescript-eslint';
import { configs as ngconfigs, processInlineTemplates } from 'angular-eslint';
import eslintConfigPrettier from 'eslint-config-prettier/flat';

export default defineConfig([
  {
    files: ['**/*.ts'],
    extends: [
      configs.recommended,
      tsconfigs.strict,
      tsconfigs.stylistic,
      ngconfigs.tsRecommended,
    ],
    processor: processInlineTemplates,
    rules: {
      '@angular-eslint/directive-selector': [
        'error',
        {
          type: 'attribute',
          prefix: 'app',
          style: 'camelCase',
        },
      ],
      '@angular-eslint/component-selector': [
        'error',
        {
          type: 'element',
          prefix: 'app',
          style: 'kebab-case',
        },
      ],
    },
  },
  {
    files: ['**/*.html'],
    extends: [ngconfigs.templateRecommended, ngconfigs.templateAccessibility],
    rules: {},
  },
  eslintConfigPrettier,
]);
