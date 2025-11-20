const SQL_ORDER = [3, 2, 1, 0, 5, 4, 7, 6, 8, 9, 10, 11, 12, 13, 14, 15];

export default class CombGuid {
  private static lastMilliseconds: bigint = 0n;
  private static sequence: number = 0; // 0..15 nibble

  public static newGuid(): string {
    const nowMilliseconds = BigInt(Date.now());
    if (nowMilliseconds === CombGuid.lastMilliseconds) {
      CombGuid.sequence = (CombGuid.sequence + 1) & 0x0f; // 0..15
    } else {
      CombGuid.lastMilliseconds = nowMilliseconds;
      CombGuid.sequence = 0;
    }

    const timestamp = new Uint8Array(8);
    let temporaryMilliseconds = nowMilliseconds;
    for (let i = 7; i >= 0; i--) {
      timestamp[i] = Number(temporaryMilliseconds & 0xffn);
      temporaryMilliseconds >>= 8n;
    }

    const bytes = CombGuid.random16();
    for (let i = 0; i < 8; i++) {
      bytes[SQL_ORDER[i]] = timestamp[i];
    }

    bytes[15] = (bytes[15] & 0xf0) | (CombGuid.sequence & 0x0f);

    const b = bytes;
    return (
      CombGuid.hex2(b[3]) +
      CombGuid.hex2(b[2]) +
      CombGuid.hex2(b[1]) +
      CombGuid.hex2(b[0]) +
      "-" +
      CombGuid.hex2(b[5]) +
      CombGuid.hex2(b[4]) +
      "-" +
      CombGuid.hex2(b[7]) +
      CombGuid.hex2(b[6]) +
      "-" +
      CombGuid.hex2(b[8]) +
      CombGuid.hex2(b[9]) +
      "-" +
      CombGuid.hex2(b[10]) +
      CombGuid.hex2(b[11]) +
      CombGuid.hex2(b[12]) +
      CombGuid.hex2(b[13]) +
      CombGuid.hex2(b[14]) +
      CombGuid.hex2(b[15])
    ).toLowerCase();
  }

  private static hex2(b: number) {
    return b.toString(16).padStart(2, "0");
  }

  private static random16(): Uint8Array {
    const b = new Uint8Array(16);
    crypto.getRandomValues(b);
    return b;
  }
}
