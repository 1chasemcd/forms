const NOT_CAPITALIZED = new Set<string>([
  'a',
  'an',
  'the',
  'and',
  'but',
  'or',
  'nor',
  'for',
  'so',
  'yet',
  'as',
  'at',
  'by',
  'in',
  'of',
  'off',
  'on',
  'per',
  'to',
  'up',
  'via',
  'from',
  'into',
  'onto',
  'upon',
  'with',
  'than',
  'till',
  'over',
  'out',
]);

export function pascalCaseToWords(input: string | null | undefined): string {
  const isUpper = (char: string) => char.toUpperCase() === char && char.toLowerCase() !== char;
  const isDigit = (char: string) => char >= '0' && char <= '9';

  if (!input) return '';

  const words: string[] = [];

  for (let i = 0; i < input.length; i++) {
    const char = input[i];

    if (i === 0 || isUpper(char) || (isDigit(char) && !isDigit(input[i - 1]))) {
      words.push('');
    }

    words[words.length - 1] += char;

    // Ensure first character is always capitalized
    if (words[words.length - 1].length === 1) {
      words[words.length - 1] = words[words.length - 1].toUpperCase();
    }
  }

  if (words.length <= 2) return words.join(' ');

  const wordsCorrected = words
    .slice(1, -1)
    .map((word) => (NOT_CAPITALIZED.has(word.toLowerCase()) ? word.toLowerCase() : word));

  return `${words[0]} ${wordsCorrected.join(' ')} ${words[words.length - 1]}`;
}
