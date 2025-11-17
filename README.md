<p align="center">
   <a href="#"><img src="https://raw.githubusercontent.com/shader-ls/shader-language-server/master/etc/logo.png" width="30%"></a>
  <h1 align="center">Shader Language Server</h1>
  <p align="center">
    <a href="https://opensource.org/licenses/MIT"><img alt="License" src="https://img.shields.io/badge/License-MIT-green.svg"/></a>
    <a href="https://github.com/shader-ls/shader-language-server/releases/latest"><img alt="Release" src="https://img.shields.io/github/tag/shader-ls/shader-language-server.svg?label=release&logo=github"/></a>
    <a href="https://www.nuget.org/packages/shader-ls/"><img alt="Nuget DT" src="https://img.shields.io/nuget/dt/shader-ls?logo=nuget&logoColor=49A2E6"/></a>
  </p>
  <p align="center">
    <a href="https://github.com/shader-ls/shader-language-server/actions/workflows/test.yml"><img alt="License" src="https://github.com/shader-ls/shader-language-server/actions/workflows/test.yml/badge.svg"/></a>
  </p>
</p>

> Language server implementation for ShaderLab

## 🚧 Status

Still in development.

#### Current Features

- Completion
- Hover
- Signature Help

#### Planned Features

- Diagnostics
- Jump to def

## 💾 Installation

`dotnet tool install --global shader-ls`

See [shader-lsp nuget page](https://www.nuget.org/packages/shader-ls/).

## 🔨 Usage

```sh
/path/to/shader-ls --stdio
```

The name is `shader-ls.exe` if you are using Windows!

## 🔧 Settings

- `ShaderLab.CompletionWord` - completing word in the completion service

## ⚜️ License

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

See [`LICENSE`](./LICENSE) for details.
