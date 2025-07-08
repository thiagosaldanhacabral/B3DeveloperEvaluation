#!/bin/bash

echo "🚀 Iniciando ambiente B3 Developer Evaluation..."
echo "================================================="

# Verificar se o Docker está rodando
if ! docker info >/dev/null 2>&1; then
    echo "❌ Docker não está rodando. Por favor, inicie o Docker e tente novamente."
    read -p "Pressione Enter para continuar..."
    exit 1
fi

# Verificar configuração do sistema
echo "🔍 Verificando configuração do sistema..."
if command -v wsl >/dev/null 2>&1; then
    echo "⚙️  Configurando WSL2 para SonarQube..."
    wsl -d docker-desktop sysctl -w vm.max_map_count=262144 >/dev/null 2>&1
    echo "✅ WSL2 configurado."
else
    echo "⚙️  Configurando vm.max_map_count para SonarQube..."
    # Para sistemas Linux nativos
    if [ -w /proc/sys/vm/max_map_count ]; then
        echo 262144 | sudo tee /proc/sys/vm/max_map_count >/dev/null
        echo "✅ vm.max_map_count configurado."
    else
        echo "⚠️  Pode ser necessário executar: sudo sysctl -w vm.max_map_count=262144"
    fi
fi

echo "🛑 Parando containers existentes..."
docker-compose down -v --remove-orphans

echo "🗑️  Removendo volumes antigos (se existirem)..."
docker volume ls -q | grep -E "(sonarqube|b3)" | xargs -r docker volume rm 2>/dev/null || true

echo "🧹 Limpando imagens antigas do SonarQube..."
docker rmi sonarqube:latest >/dev/null 2>&1 || true
docker rmi sonarqube:10.7-community >/dev/null 2>&1 || true

echo "🔨 Construindo imagens..."
docker-compose build --no-cache

echo ""
echo "🎯 Iniciando serviços em sequência:"
echo "   1. SonarQube será iniciado primeiro"
echo "   2. Aguardará o SonarQube estar saudável"
echo "   3. API será iniciada após configuração do SonarQube"
echo "   4. UI será iniciada após API estar saudável"
echo ""

# Iniciar todos os serviços (com dependências automáticas)
docker-compose up -d

echo ""
echo "📊 Monitorando progresso da inicialização..."
echo ""

# Função para aguardar serviço ficar saudável
wait_for_healthy() {
    local service_name=$1
    local display_name=$2
    
    echo "🔍 Verificando status do $display_name..."
    while ! docker-compose ps "$service_name" | grep -q "healthy"; do
        echo "   ⏳ $display_name ainda não está pronto. Aguardando..."
        sleep 15
    done
    echo "✅ $display_name está pronto!"
}

# Aguardar serviços ficarem saudáveis
wait_for_healthy "sonarqube-b3" "SonarQube"
wait_for_healthy "b3-developer-evaluation-api" "API"
wait_for_healthy "b3-developer-evaluation-ui" "UI"

echo ""
echo "🎉 Todos os serviços foram iniciados com sucesso!"
echo ""
echo "📋 Status atual dos containers:"
docker-compose ps

echo ""
echo "🌐 URLs dos serviços:"
echo "   SonarQube: http://localhost:9000"
echo "   API: http://localhost:5100"
echo "   UI: http://localhost:4200"
echo ""
echo "🔧 Comandos úteis:"
echo "   docker-compose logs -f                    # Ver logs de todos os serviços"
echo "   docker-compose logs -f sonarqube-b3       # Ver logs do SonarQube"
echo "   docker-compose logs -f sonarqube-config   # Ver logs da configuração"
echo "   docker-compose ps                         # Ver status dos containers"
echo ""
echo "🚀 Ambiente pronto para uso!"
echo ""
echo "Pressione Enter para continuar..."
read