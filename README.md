# Tucil2_13524071_13524109

## Deskripsi
Program C# untuk melakukan voxelization objek 3D berbasis file `.obj` menggunakan struktur data Octree. Program ini mampu membangun representasi voxel dari mesh segitiga serta menampilkan hasilnya melalui viewer.

## Requirement
- .NET SDK 8.0 atau versi yang kompatibel
- OS: Windows (karena menggunakan Windows Forms untuk viewer)

## Struktur Folder
- `src/`: Source code utama program
- `bin/` : Executable file
- `test/` : File input dan output untuk pengujian
  - `input/` : File `.obj` sebagai input
  - `output/` : Hasil voxelization (`.obj`)
- `doc/` : Laporan tugas

## Cara Build
Masuk ke folder `src`, lalu jalankan:

```bash
dotnet build
```

## Cara Menjalankan

### 1. Voxelization
```bash
dotnet run -- <input.obj> <maxDepth> [output.obj] [parallel]
```
**Parameter:**
- `input.obj`   : File objek 3D
- `maxDepth`    : Kedalaman maksimum octree
- `output.obj` (opsional) : Nama file output (default: output.obj)
- `parallel` (opsional) : Gunakan mode paralel

### 2. Viewer
Untuk melihat hasil `.obj`:
```bash
dotnet run -- view <file.obj>
```

## Contoh Penggunaan

### Sequential
```bash
dotnet run -- ../test/input/cat.obj 8
```

### Parallel
```bash
dotnet run -- ../test/input/cat.obj 8 ..test/output/cat_output.obj parallel                       
```

### Viewer
```bash
dotnet run -- view cat_output.obj     
```

## Catatan
- Semakin besar `maxDepth`, semakin detail voxel, tetapi ukuran file output `.obj` juga meningkat.
- File `.obj` dengan face non-segitiga akan diabaikan oleh parser.

## Author

| NIM | Nama |
|---|---|
| 13524071 | Kalyca Nathania B. Manullang |
| 13524109 | Helena Kristela Sarhawa |