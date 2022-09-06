#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# soft link
RUN ln -s /lib/x86_64-linux-gnu/libdl-2.24.so /lib/x86_64-linux-gnu/libdl.so

# install System.Drawing native dependencies
RUN apt-get update \
    && apt-get install -y --allow-unauthenticated \
   		libgdiplus \
#         libc6-dev \
#         libgdiplus \
#         libx11-dev \
     && rm -rf /var/lib/apt/lists/*

FROM emscripten/emsdk:3.0.1 as build
RUN wget https://packages.microsoft.com/config/ubuntu/21.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb; dpkg -i packages-microsoft-prod.deb;rm packages-microsoft-prod.deb
RUN apt-get update; apt-get install -y apt-transport-https && \
    apt-get update && \
    apt-get install -y dotnet-sdk-6.0
WORKDIR /src
RUN dotnet workload install wasm-tools
COPY ["Server/e_CommerceContinental.Server.csproj", "Server/"]
COPY ["Client/e_CommerceContinental.Client.csproj", "Client/"]
COPY ["Shared/e_CommerceContinental.Shared.csproj", "Shared/"]
RUN dotnet restore "Server/e_CommerceContinental.Server.csproj"
COPY . .
WORKDIR "/src/Server"
RUN dotnet build "e_CommerceContinental.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "e_CommerceContinental.Server.csproj" -c Release -o /app/publish


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet e_CommerceContinental.Server.dll