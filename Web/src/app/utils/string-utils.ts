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

const DAYS_IN_MONTH: Record<string, number> = {
  '1': 31,
  '2': 29,
  '3': 31,
  '4': 30,
  '5': 31,
  '6': 30,
  '7': 31,
  '8': 31,
  '9': 30,
  '10': 31,
  '11': 30,
  '12': 31,
};

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

export function formatAsDate(value: string): string {
  const validDaysForMonth = (month: string) => {
    const count = DAYS_IN_MONTH[month] ?? 31;
    return Array.from({ length: count }, (_, i) => String(i + 1));
  };

  value = value.replace(/[^/\d]/g, '').replace(/^[0/]+/, '');

  let month = '';
  let sep1 = '';
  let day = '';
  let sep2 = '';
  let year = '';

  // month
  if (value && ['10', '11', '12'].includes(value.slice(0, 2))) {
    month = value.slice(0, 2);
    value = value.slice(2);
  } else {
    month = value.slice(0, 1);
    value = value.slice(1);
  }

  // '/'
  if (value) sep1 = '/';
  value = value.replace(/^[0/]+/, '');

  // day
  if (value && validDaysForMonth(month).includes(value.slice(0, 2))) {
    day = value.slice(0, 2);
    value = value.slice(2);
  } else {
    day = value.slice(0, 1);
    value = value.slice(1);
  }

  // '/'
  if (value) sep2 = '/';
  value = value.replace(/^\/+/, '');

  // year
  year = value.replace(/\D/g, '').slice(0, 4);

  return `${month}${sep1}${day}${sep2}${year}`;
}

export function mapIndices(initial: string, final: string): (number | null)[] {
  const m = initial.length;
  const n = final.length;

  // Build LCS table
  const dp = Array.from({ length: m + 1 }, () => Array(n + 1).fill(0));

  for (let i = m - 1; i >= 0; i--) {
    for (let j = n - 1; j >= 0; j--) {
      if (initial[i] === final[j]) {
        dp[i][j] = dp[i + 1][j + 1] + 1;
      } else {
        dp[i][j] = Math.max(dp[i + 1][j], dp[i][j + 1]);
      }
    }
  }

  // Reconstruct mapping
  const result = Array(m).fill(null);

  let i = 0;
  let j = 0;

  while (i < m && j < n) {
    if (initial[i] === final[j]) {
      result[i] = j;
      i++;
      j++;
    } else if (dp[i + 1][j] >= dp[i][j + 1]) {
      i++;
    } else {
      j++;
    }
  }

  return result;
}
